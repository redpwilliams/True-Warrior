using System.Collections;
using JetBrains.Annotations;
using Managers;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using Util;
using Random = UnityEngine.Random;

namespace Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Light2D))]
    public abstract class Character : MonoBehaviour, IControllable
    {
        // Components
        protected Rigidbody2D Rb2d;
        protected Animator Anim;
        private Transform _transform;
        private Controls _controls;
        private PlayerInputFlags _playerInputFlags;

        public float EndPosition { get; set; }
    
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
        private static readonly int SetWithDelay = Animator.StringToHash("ShouldSet_Delay");
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
            
            // Create and enable all controls by default
            _controls = new Controls();
            _controls.Player1.Enable(); // Enable the entire control map
            _playerInputFlags = PlayerInputFlags.None; // Ensure specific controls are disabled
            // Individual controls initially disabled in Start()

            _characterText = GetComponentInChildren<CharacterText>();
        }

        private void OnDisable()
        {
            if (_playerNumber == PlayerNumber.CPU) return;
            _controls.Disable(); // maybe redundant but maybe necessary
            _controls.Player1.Attack.performed -= OnControllerInput;
        }

        protected virtual void Start()
        {
            EventManager.Events.OnRestartCurrentGameMode += DestroySelf;
            
            // TODO - Subscribe to GameManager's events
            GameManager.OnEnablePlayerControls += (number, flags) =>
            {
                // Disregard if this event wasn't meant for this Player
                if (number != _playerNumber) return;
                EnableInput(flags);
            };

            GameManager.OnDisablePlayerControls += (number, flags) =>
            {
                // Disregard if this event wasn't meant for this Player
                if (number != _playerNumber) return;
                DisableInput(flags);
            };
            
            // Start with all individual controls disabled
            DisableInput((PlayerInputFlags) 0b111);
        }

        private void OnDestroy()
        {
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

        public void RunToSet(HaikuStage haikuStage)
        {
            // Bail out if its not this Character's turn to run to set
            StandoffState expectedStage = _playerNumber == PlayerNumber.One
                ? StandoffState.Line1
                : StandoffState.Line2;
            if (haikuStage.state != expectedStage) return;
        
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
                
                // Turn on controls
                EnableInput(PlayerInputFlags.Scroll, PlayerInputFlags.Select);
            }
        }
        private int GetDirection() => (int) Mathf.Sign(_transform.localScale.x);

        #endregion

        #region Battle

        /// Puts the CPU in the "Set" position after the
        /// third hs choices of the Haiku is revealed. 
        public void GetSetCPU(HaikuStage haikuStage)
        {
            if (_playerNumber != PlayerNumber.CPU) return;
            if (haikuStage.state != StandoffState.Ready_CPU) return;
            StartCoroutine(AnimateIdleToSet());
        }

        /// Puts the player in the "Set" position after
        /// selecting the third hs of the Haiku
        public void GetSetPlayer(HaikuStage hs)
        {
            if (_playerNumber == PlayerNumber.CPU) return;
            if (hs.state != StandoffState.Ready_Player) return;
            StartCoroutine(AnimateIdleToSet());
            
            // Player must hold down attack trigger now
            EnableBattleControls();
        }
        
        private IEnumerator AnimateIdleToSet()
        {
            // If CPU, wait a bit before getting set
            if (_playerNumber == PlayerNumber.CPU)
            { 
                yield return new WaitForSeconds(Random.Range(0f, 0.25f)); 
                Anim.SetTrigger(SetWithDelay);
                yield break; 
            }
            
            // If Player, start animation right away without exit time
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
            // GameManager.Manager.FinishGameMode();
        }

        #endregion

        #region Input

        /// Does nothing if input is PlayerInputFlags.None.
        public void EnableInput(PlayerInputFlags input)
        {
            if (input == PlayerInputFlags.None) return;
            
            if (input.HasFlag(PlayerInputFlags.Scroll))
            {
                _controls.Player1.Scroll.Enable();
            }
            
            if (input.HasFlag(PlayerInputFlags.Select))
            {
                _controls.Player1.Select.Enable();
            }
            
            if (input.HasFlag(PlayerInputFlags.Attack))
            {
                _controls.Player1.Attack.Enable();
            }

            _playerInputFlags |= input;
        }

        /// Accepts an optional number of parameters to disable Player controls.
        /// Does nothing if input is PlayerInputFlags.None.
        public void EnableInput(params PlayerInputFlags[] inputs)
        {
            if (inputs.Length == 0) return;

            foreach (var input in inputs)
                EnableInput(input);
        }

        /// Does nothing if input is PlayerInputFlags.None.
        public void DisableInput(PlayerInputFlags input)
        {
            if (input == PlayerInputFlags.None)
                return;
            
            if (input.HasFlag(PlayerInputFlags.Scroll))
            {
                _controls.Player1.Scroll.Disable();
                _playerInputFlags &= ~PlayerInputFlags.Scroll;
            }

            if (input.HasFlag(PlayerInputFlags.Select))
            {
                _controls.Player1.Select.Disable();
                _playerInputFlags &= ~PlayerInputFlags.Select;
            }
            
            // ReSharper disable once InvertIf
            if (input.HasFlag(PlayerInputFlags.Attack))
            {
                _controls.Player1.Attack.Disable();
                _playerInputFlags &= ~PlayerInputFlags.Attack;
            }
        }

        /// Accepts an optional number of parameters to enable Player controls.
        /// Any input of PlayerInputFlags.None does nothing.
        public void DisableInput(params PlayerInputFlags[] inputs)
        {
            if (inputs.Length == 0) return;

            foreach (var input in inputs)
                DisableInput(input);
        }

        /// Subscribes this Character to the Attack.performed event.
        /// Only applies if this Character is not a CPU.
        public void RegisterControls()
        {
            if (_playerNumber == PlayerNumber.CPU) return;
            _controls.Player1.Attack.performed += OnControllerInput;
            _controls.Player1.Scroll.performed += GameManager.Manager.OnHaikuScroll;
            _controls.Player1.Select.performed += GameManager.Manager.OnHaikuSelect;
        }

        /// Enables controls strictly relating to attacking. Not only does this
        /// enable the Attack control, but it also alters the RigidBody2D to
        /// enable physics as a result of such attacks.
        private void EnableBattleControls()
        {
            // Enable controls
            Rb2d.bodyType = RigidbodyType2D.Dynamic;
            if (_playerNumber != PlayerNumber.CPU)
            {
                // REVIEW - Only allows attack controls
                EnableInput(PlayerInputFlags.Attack);
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