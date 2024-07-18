using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

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
    private enum Player { One, Two, CPU }
    [SerializeField] private Player _player;
    
    // Attacking Animation
    protected static readonly int Attacking = Animator.StringToHash
        ("ShouldAttack");

    // private AnimationHandler _ah;

    private void Awake()
    {
        Rb2d = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        _controls = new Controls();
    }

    private void OnEnable()
    {
        if (_player == Player.CPU) return;
        _controls.Player1.Attack.performed += OnCharacterInput;
    }

    private void OnDisable()
    {
        if (_player == Player.CPU) return;
        _controls.Disable(); // maybe redundant but maybe necessary
        _controls.Player1.Attack.performed -= OnCharacterInput;
    }

    protected virtual void Start()
    {
        EventManager.Events.OnStageX += StartRunning;
        EventManager.Events.OnStageX += EnableControls;
        EventManager.Events.OnBeginAttack += Attack;
        
        
        // Correct final position
        _finalPosition = (_player == Player.One)
            ? -Mathf.Abs(_finalPosition)
            : Mathf.Abs(_finalPosition);
        
        // Flip sprite
        if (_player == Player.One) return;
        
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
        if ((_player == Player.One && Rb2d.position.x < _finalPosition) || 
        _player != Player.One && Rb2d.position.x > _finalPosition) return;
        
        // Stop the movement (set velocity to zero)
        Rb2d.velocity = Vector2.zero;
        _isRunning = false;
        Anim.SetBool(Running, _isRunning);
    }

    #region ApproachingMovement
    
    private void StartRunning(int stage)
    {
        if (stage != (_player == Player.One ? 0 : 1)) return;
        
        // Start Character movement
        _isRunning = true;
        Anim.SetBool(Running, _isRunning);
        EventManager.Events.OnStageX -= StartRunning;
    }

    #endregion

    #region Battle

    // wins
    protected virtual void Attack()
    {
       Debug.Log("Beginning attack");
       // EventManager.Events.CharacterAttacks();
       Anim.SetBool(Attacking, true);
    }
    
    // Immediately losing toss-up
    protected virtual void LostToAttack()
    {
        Debug.Log("Lost to attack");
    }

    #endregion

    #region Input

    // this character has inputted the attack button
    private void OnCharacterInput(InputAction.CallbackContext context)
    {
        // Send info to game manager with timestamp
        Character winner = EventManager.Events.PlayerInput(this, context.time); 
        // asks EM if its the winner
        if (this == winner)
        {
            Debug.Log("I won");
            return;
        }
        Debug.Log("I lost");
    }

    // Disabling logic happens in EventManager, after a winner is determined
    private void EnableControls(int stage)
    {
        // Disregard if battle start hasn't been called
        if (stage != 3) return; 
        
        // Enable controls
        if (_player != Player.CPU)
        {
            _controls.Player1.Enable();
            return;
        }
        
        StartCoroutine(DelayCPUAttack());
    }

    private IEnumerator DelayCPUAttack()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        // Send info to game manager with timestamp
        Character winner = EventManager.Events.CPUInput(this, Time.realtimeSinceStartupAsDouble); 
        // asks EM if its the winner
        if (this == winner)
        {
            Debug.Log("I won");
            yield break;
        }
        Debug.Log("I lost");

    }

    public void DisableControls()
    {
        _controls.Player1.Disable();
    }

    #endregion
}
public interface IReactive
{ 
    
    [UsedImplicitly]
    public void OnFinishAttack();

}

