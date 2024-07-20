using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Light2D))]
    public abstract class Character : MonoBehaviour
    {
        // Components
        protected Rigidbody2D Rb2d;
        protected Animator Anim;
        private Light2D _l2d;
        private Controls _controls;
    
        // Running Animation
        private static readonly int Running = Animator.StringToHash("ShouldRun");
        [SerializeField] private float _finalPosition;
        [SerializeField] private float _speed = 2f;
        private bool _isRunning;
    
        // Assigned stage
        protected enum PlayerType { One, Two, CPU }
        [SerializeField] protected PlayerType _playerType;
        
        // Set Animation
        private static readonly int Set = Animator.StringToHash("ShouldSet");
    
        // Attacking Animation
        protected static readonly int Attacking = Animator.StringToHash
            ("ShouldAttack");
        
        // Hurt Animation
        private static readonly int Hurt = Animator.StringToHash("ShouldHurt");
        
        // Death Animation
        private static readonly int Death = Animator.StringToHash("ShouldDie");
        
        // Return to Idle
        protected static readonly int Return =
            Animator.StringToHash("ShouldReturn");

        protected Character Opponent;

        private void Awake()
        {
            Rb2d = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
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
        
            // Correct final position
            _finalPosition = (_playerType == PlayerType.One)
                ? -Mathf.Abs(_finalPosition)
                : Mathf.Abs(_finalPosition);
        
            // Flip sprite
            if (_playerType == PlayerType.One) return;
        
            var transform1 = transform;
            var localScale = transform1.localScale;
            localScale = new Vector2(-1 * localScale.x, localScale.y);
            transform1.localScale = localScale;
        }

        private void FixedUpdate()
        {
            // Disregard if not moving
            if (!_isRunning) return;
        
            // Move the character towards the final position
            Vector2 currentPosition = Rb2d.position;
            Vector2 targetPosition = new Vector2(_finalPosition, currentPosition.y);
            Rb2d.MovePosition(Vector2.MoveTowards(currentPosition, targetPosition,
                _speed * Time.fixedDeltaTime));

            // Check if the character has reached or passed the final position
            if ((_playerType == PlayerType.One && Rb2d.position.x < _finalPosition) || 
                _playerType != PlayerType.One && Rb2d.position.x > _finalPosition) return;
        
            // Stop the movement (set velocity to zero)
            Rb2d.velocity = Vector2.zero;
            _isRunning = false;
            Anim.SetBool(Running, _isRunning);
        }

        #region ApproachingMovement
    
        private void StartRunning(int stage)
        {
            if (stage != (_playerType == PlayerType.One ? 0 : 1)) return;
        
            // Start Character movement
            _isRunning = true;
            Anim.SetBool(Running, _isRunning);
            EventManager.Events.OnStageX -= StartRunning;
        }

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
            yield return new WaitForSeconds(Random.Range(0f, 0.75f));
            Anim.SetTrigger(Set);
        }

        // wins
        protected abstract void Attack();
    
        // Immediately losing toss-up
        protected abstract void LostToAttack();

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
            if (this == winner)
            {
                Attack();
                return;
            }  
            LostToAttack();
        }


        #endregion

        public void DoHurtAnimation(float hurtPushForce)
        {
            Anim.ResetTrigger(Hurt);
            Anim.SetTrigger(Hurt);
            Rb2d.AddForce(new Vector2(hurtPushForce, 0f), ForceMode2D.Impulse);
        }

        public void DoDeathAnimation()
        {
            Anim.SetTrigger(Death);
        }
    }
}