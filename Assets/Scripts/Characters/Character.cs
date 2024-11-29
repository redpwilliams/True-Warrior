using System.Collections;
using JetBrains.Annotations;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Util;
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
        private Controls _controls;

        public float EndPosition { get; set; }
    
        public enum PlayerNumber { One, Two, CPU }
        
        [FormerlySerializedAs("_playerType")]
        [Header("Player Designation")]
        [SerializeField] private PlayerNumber _playerNumber;
        
        /// The PlayerNumber of this character
        public PlayerNumber Player
        {
            get => _playerNumber;
            set => _playerNumber = value;
        }
        
        // Animation parameters
        private static readonly int Running = Animator.StringToHash("ShouldRun");
        private static readonly int Set = Animator.StringToHash("ShouldSet");
        private static readonly int Attacking = Animator.StringToHash("ShouldAttack");
        private static readonly int Hurt = Animator.StringToHash("ShouldHurt");
        private static readonly int Death = Animator.StringToHash("ShouldDie");
        private static readonly int Return = Animator.StringToHash("ShouldReturn");

        public Character Opponent { get; set; }
        private CharacterText _characterText;
        private ReactionInfo _battleData;
        private bool _firstContact = true;

        private void Awake()
        {
            Rb2d = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();

            _transform = transform;
            _controls = new Controls();

            _characterText = GetComponentInChildren<CharacterText>();
        }

        private void OnEnable()
        {
            if (_playerNumber == PlayerNumber.CPU) return;
            _controls.Player1.Attack.performed += OnControllerInput;
        }

        private void OnDisable()
        {
            if (_playerNumber == PlayerNumber.CPU) return;
            _controls.Disable(); // maybe redundant but maybe necessary
            _controls.Player1.Attack.performed -= OnControllerInput;
        }

        protected virtual void Start()
        {
            EventManager.Events.OnStageX += RunToSet;
            EventManager.Events.OnStageX += AllowControls;
            EventManager.Events.OnStageX += GetSet;
            EventManager.Events.OnRestartCurrentGameMode += DestroySelf;
        }

        private void OnDestroy()
        {
            EventManager.Events.OnStageX -= RunToSet;
            EventManager.Events.OnStageX -= AllowControls;
            EventManager.Events.OnStageX -= GetSet;
            EventManager.Events.OnRestartCurrentGameMode -= DestroySelf;
        }

        #region Movement and Positioning

        /// Sets this Character's position in the scene.
        /// Assumes its PlayerNumber is updated and accurate.
        public void SetPosition()
        {
            Transform trans = transform;
            if (_playerNumber == PlayerNumber.One)
            { 
                trans.position = new Vector3(
                    InitParams.Standoff_P1_StartPositionX, 
                    InitParams.Standoff_P1_StartPositionY, 
                    InitParams.Standoff_P1_StartPositionZ);
                return;
            }
            
            trans.position = new Vector3(
                InitParams.Standoff_PX_StartPositionX, 
                InitParams.Standoff_PX_StartPositionY, 
                InitParams.Standoff_PX_StartPositionZ);
        }

        /// Sets this Character's orientation in the scene.
        /// All sprites, by default, face towards the right.
        /// Assumes its PlayerNumber is updated and accurate.
        public void SetDirection()
        {
            // Already facing the correct direction
            // if PlayerNumber is One
            if (_playerNumber == PlayerNumber.One) return;
            
            // Cache transform reference
            Transform trans = transform;
            
            // Set sprite direction
            var localScale = trans.localScale;
            localScale = 
                new Vector2(-1 * Mathf.Abs(localScale.x), localScale.y);
            trans.localScale = localScale;

            // Set character text direction
            _characterText.SetDirection();
        }
        
        private void RunToSet(int stage)
        {
            if (stage != (_playerNumber == PlayerNumber.One ? 0 : 1)) return;
        
            // Unsubscribe from triggering event
            EventManager.Events.OnStageX -= RunToSet;

            StartCoroutine(Run());

            IEnumerator Run()
            {
                // Move the character towards the final position
                Anim.SetBool(Running, true);
                float runDistance = Mathf.Abs(Rb2d.position.x - EndPosition);
                float moveDuration = 1.3f;
                yield return QuickMove(runDistance, moveDuration, function: Lerp2D.NoEase);
                Anim.SetBool(Running, false);
                
                // Set character title
                string title = (_playerNumber == PlayerNumber.One)
                    ? "You/君"
                    : CharacterTitle();
                _characterText.DisplayTitle(title);
            }
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

        protected virtual void Attack() => Anim.SetBool(Attacking, true);

        [UsedImplicitly]
        public virtual void OnStrikeTarget(int isFinalHit)
        {
            if (_firstContact)
            {
                // Tell the opponent to show late
                Opponent.ShowReactionTimeAsLate();
                _firstContact = false;
            }
            StartCoroutine(ReturnToIdle(3f));
        } 

        [UsedImplicitly]
        public virtual void OnFinishAttack() => Anim.SetBool(Attacking, false);
        
        private IEnumerator ReturnToIdle(float time)
        {
            yield return new WaitForSeconds(time);
            Anim.SetTrigger(Return);
            GameManager.Manager.FinishGameMode();
        }

        #endregion

        #region Input

        /// Subscribes this Character to the Attack.performed event.
        /// Only applies if this Character is not a CPU.
        public void RegisterControls()
        {
            if (_playerNumber == PlayerNumber.CPU) return;
            _controls.Player1.Attack.performed += OnControllerInput;
        }

        // Disabling logic happens in EventManager, after a winner is determined
        private void AllowControls(int stage)
        {
            // Disregard if battle start hasn't been called
            if (stage != 3) return; 
        
            // Enable controls
            Rb2d.bodyType = RigidbodyType2D.Dynamic;
            if (_playerNumber != PlayerNumber.CPU)
            {
                _controls.Player1.Enable();
                return;
            }
        
            StartCoroutine(DelayCPUAttack());
        }
    
        /// Fires from an input defined in the Controls InputAction map
        /// when a Character - a player or CPU - attacks.
        private void OnControllerInput(InputAction.CallbackContext context)
        {
            _battleData = GameManager.Manager.AttackInput(Time.time); 
            _controls.Player1.Disable();
            
            _characterText.ShowReactionTime(_battleData.ReactionTime);
            DetermineReactionAnimation(_battleData.IsWinner);
        }

        private IEnumerator DelayCPUAttack()
        {
            // CPU Attack Delay
            // TODO - Make 3 ranges for easy, medium, and hard
            yield return new WaitForSeconds(0.5f); // TODO "speed tiers" ?
            
            // Attack, and ask GameManager for results
            _battleData  = GameManager.Manager.AttackInput(Time.time); 
            
            _characterText.ShowReactionTime(_battleData.ReactionTime);
            DetermineReactionAnimation(_battleData.IsWinner);
        }

        private void DetermineReactionAnimation(bool isWinner)
        {
            if (isWinner) Attack();
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

        protected delegate float Lerp2DFunction(float t);
        
        /// Moves Character from one position to the other. Configurable
        /// as knockback, and with lerping function (EaseOutQuart is default).
        /// By default, the Character moves forward in the direction it's facing.
        protected IEnumerator QuickMove(float moveDistance, float moveDuration, 
            bool isKnockback = false, Lerp2DFunction function = null)
        {
            // Push direction
            int direction = GetDirection();
            if (isKnockback) direction *= -1;
            
            // Determine final position
            Vector2 currentPosition = Rb2d.position;
            Vector2 targetPosition = new Vector2(
                currentPosition.x + direction * moveDistance, 
                currentPosition.y);
            
            // Determine timing function
            function ??= Lerp2D.EaseOutQuart; // EaseOutQuart is default

            float elapsedTime = 0f;
            while (elapsedTime < moveDuration)
            {
                float t = elapsedTime / moveDuration;

                var position = Vector2.Lerp(currentPosition, targetPosition, function(t));
                Rb2d.MovePosition(position);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Rb2d.position = targetPosition;
        }

        public void DoDeathAnimation() => Anim.SetTrigger(Death);

        #endregion

        #region Character Texts

        protected abstract string CharacterTitle();

        private void ShowReactionTimeAsLate()
        {
            _characterText.ShowReactionTime("Late");
            _controls.Disable();
        }

        #endregion

        private void DestroySelf()
        {
            Destroy(this.gameObject);
        }
    }
}