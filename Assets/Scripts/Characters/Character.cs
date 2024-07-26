using System.Collections;
using JetBrains.Annotations;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Light2D))]
    public abstract class Character : MonoBehaviour
    {
        // Components
        protected Rigidbody2D Rb2d;
        protected Animator Anim;
        private Transform _transform;
        private Light2D _l2d;
        private Controls _controls;

        [Header("Game Start Running Parameters")]
        private readonly float _runSpeed = InitParams.RunSpeed;
        private float _endPosition = InitParams.EndPositionX;
        private bool _isRunning;
    
        [Header("Player Designation")]
        [SerializeField] protected PlayerType _playerType;
        protected enum PlayerType { One, Two, CPU }
        
        // Animation parameters
        // TODO - Use Scriptable Objects
        private static readonly int Running = Animator.StringToHash("ShouldRun");
        private static readonly int Set = Animator.StringToHash("ShouldSet");
        private static readonly int Attacking = Animator.StringToHash("ShouldAttack");
        private static readonly int Hurt = Animator.StringToHash("ShouldHurt");
        private static readonly int Death = Animator.StringToHash("ShouldDie");
        private static readonly int Return = Animator.StringToHash("ShouldReturn");

        protected Character Opponent;

        private void Awake()
        {
            Rb2d = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();

            _l2d = GetComponent<Light2D>();
            _l2d.intensity = InitParams.Light2DBright;
            
            _transform = transform;
            _controls = new Controls();
        }

        private void OnEnable()
        {
            if (_playerType == PlayerType.CPU) return;
            _controls.Player1.Attack.performed += OnControllerInput;
        }

        private void OnDisable()
        {
            if (_playerType == PlayerType.CPU) return;
            _controls.Disable(); // maybe redundant but maybe necessary
            _controls.Player1.Attack.performed -= OnControllerInput;
        }

        protected virtual void Start()
        {
            EventManager.Events.OnStageX += StartRunning;
            EventManager.Events.OnStageX += AllowControls;
            EventManager.Events.OnStageX += GetSet;
            EventManager.Events.OnBeginAttack += Attack;
            
            // Find opponent
            Character[] players =
                FindObjectsByType<Character>(FindObjectsSortMode.None);
            Opponent = (players[0] == this) ? players[1] : players[0];
        
            // Set position and sprite direction
            var localScale = _transform.localScale;
            localScale.x = InitParams.StartPositionX;
            if (_playerType == PlayerType.One)
            {
                _endPosition = -Mathf.Abs(InitParams.EndPositionX);
            }
            else
            {
                localScale.x = -Mathf.Abs(localScale.x);
            }
        }

        private void FixedUpdate()
        {
            // Disregard if not moving
            if (!_isRunning) return;
        
            // Move the character towards the final position
            Vector2 currentPosition = Rb2d.position;
            Vector2 targetPosition = new Vector2(_endPosition, currentPosition.y);
            Rb2d.MovePosition(Vector2.MoveTowards(currentPosition, targetPosition,
                _runSpeed * Time.fixedDeltaTime));

            // Check if the character has reached or passed the final position
            // TODO - Revisit
            if ((_playerType == PlayerType.One && Rb2d.position.x < _endPosition) || 
                _playerType != PlayerType.One && Rb2d.position.x > _endPosition) return;
        
            _isRunning = false;
            Rb2d.velocity = Vector2.zero;
            Rb2d.position = targetPosition;
            Anim.SetBool(Running, _isRunning);
        }

        #region Movement and Positioning
    
        private void StartRunning(int stage)
        {
            if (stage != (_playerType == PlayerType.One ? 0 : 1)) return;
        
            // Start Character movement
            _isRunning = true;
            Anim.SetBool(Running, _isRunning);
            EventManager.Events.OnStageX -= StartRunning;
        }
        
        private int GetDirection() => (int) Mathf.Sign(_transform.localScale.x);

        #endregion

        #region Battle

        private void GetSet(int stage)
        {
            if (stage != 2) return;
            StartCoroutine(DelayIdleToSet());
            EventManager.Events.OnStageX -= GetSet;
        }

        private IEnumerator DelayIdleToSet()
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.25f));
            Anim.SetTrigger(Set);
        }

        private void Attack() => Anim.SetBool(Attacking, true);

        [UsedImplicitly]
        public virtual void OnStrikeTarget(int isFinalHit) => StartCoroutine(ReturnToIdle(3f));

        [UsedImplicitly]
        public virtual void OnFinishAttack() => Anim.SetBool(Attacking, false);
        
        private IEnumerator ReturnToIdle(float time)
        {
            yield return new WaitForSeconds(time);
            Anim.SetTrigger(Return);
        }


        #endregion

        #region Input

        // Disabling logic happens in EventManager, after a winner is determined
        private void AllowControls(int stage)
        {
            // Disregard if battle start hasn't been called
            if (stage != 3) return; 
        
            // Enable controls
            Rb2d.bodyType = RigidbodyType2D.Dynamic;
            if (_playerType != PlayerType.CPU)
            {
                _controls.Player1.Enable();
                return;
            }
        
            StartCoroutine(DelayCPUAttack());
        }
    
        // this character has inputted the attack button
        private void OnControllerInput(InputAction.CallbackContext context)
        {
            Character winner = EventManager.Events.CharacterInputsAttack(this, context.time); 
            _controls.Player1.Disable();
            DetermineReactionAnimation(winner);
        }

        private IEnumerator DelayCPUAttack()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            Character winner = EventManager.Events.CharacterInputsAttack(this, Time.realtimeSinceStartupAsDouble); 
            DetermineReactionAnimation(winner);
        }

        private void DetermineReactionAnimation(Character winner)
        {
            if (this == winner) Attack();
        }

        #endregion

        #region Hurt/Death

        // Without knockback
        public void DoHurtAnimation()
        {
            Anim.ResetTrigger(Hurt);
            Anim.SetTrigger(Hurt);
        }
        
        // With knockback
        public void DoHurtAnimation(float knockbackDistance, float knockbackDuration)
        {
            DoHurtAnimation();
            StartCoroutine(QuickMove(knockbackDistance, knockbackDuration, true));
        }

        protected IEnumerator QuickMove(float moveDistance, float 
            moveDuration, bool isKnockback = false)
        {
            // Push direction
            int direction = GetDirection();
            if (isKnockback) direction *= -1;
            
            // Determine final position
            Vector2 currentPosition = Rb2d.position;
            Vector2 targetPosition = new Vector2(
                currentPosition.x + direction * moveDistance, 
                currentPosition.y);

            float elapsedTime = 0f;
            while (elapsedTime < moveDuration)
            {
                float t = elapsedTime / moveDuration;

                var position = Vector2.Lerp(currentPosition, targetPosition,
                    Lerp2D.EaseOutQuart(t));
                Rb2d.MovePosition(position);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Rb2d.position = targetPosition;
        }

        public void DoDeathAnimation() => Anim.SetTrigger(Death);

        public void OnDeathAnimationFinish()
        {
            float startBrightness = _l2d.intensity;
            float endBrightness = InitParams.Light2DDim;

            StartCoroutine(DimAfterDeath());

            IEnumerator DimAfterDeath()
            {
                float elapsedTime = 0f;
                
                while (elapsedTime < InitParams.DimDuration)
                {
                    float t = elapsedTime / InitParams.DimDuration;

                    _l2d.intensity = Mathf.Lerp(startBrightness, endBrightness, t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                
                yield return null;
            }
        }
        
        #endregion

        #region Context Menu Actions

        [ContextMenu("Set Character Starting Position")]
        private void SetCharacterStartPosition()
        {
            int startPositionSign = _playerType == PlayerType.One ? -1 : 1;
            Transform trans = transform;
            trans.position = new Vector3(
                startPositionSign * InitParams.StartPositionX,
                InitParams.StartPositionY,
                InitParams.StartPositionZ);

            var localScale = trans.localScale;
            localScale = new Vector2(-startPositionSign * Mathf.Abs(localScale.x), 
            localScale.y);
            trans.localScale = localScale;
        }

        #endregion
    }
}