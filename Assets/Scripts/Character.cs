using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Character : MonoBehaviour
{
    // Components
    protected Rigidbody2D Rb2d;
    protected Animator Anim;
    private SpriteRenderer _sr;
    
    // Running Animation
    private static readonly int Running = Animator.StringToHash("ShouldRun");
    [SerializeField] private float _finalPosition;
    [SerializeField] private float _speed = 2f;
    private bool _isRunning;
    
    // Assigned stage
    [SerializeField] private Player _player;
    
    // Attacking Animation
    protected static readonly int Attacking = Animator.StringToHash
    ("ShouldAttack");

    private void Awake()
    {
        Rb2d = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }
    

    protected virtual void Start()
    {
        EventManager.Events.OnStageX += StartRunning;
        EventManager.Events.OnBeginAttack += Attack;
        
        // Correct final position
        _finalPosition = (_player == Player.One)
            ? -Mathf.Abs(_finalPosition)
            : Mathf.Abs(_finalPosition);
        
        // Flip sprite
        if (_player != Player.One) _sr.flipX = true;
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
    
    private void StartRunning(int stage)
    {
        if (stage != (_player == Player.One ? 0 : 1)) return;
        
        // Start Character movement
        _isRunning = true;
        Anim.SetBool(Running, _isRunning);
        EventManager.Events.OnStageX -= StartRunning;
    }

    // wins
    protected virtual void Attack()
    {
       Debug.Log("Beginning attack");
       EventManager.Events.CharacterAttacks();
       Anim.SetBool(Attacking, true);
    }
    
    // Immediately losing toss-up
    protected virtual void LostToAttack()
    {
        Debug.Log("Lost to attack");
    }

    private enum Player
    {
        One,
        Two,
        CPU
    }

}
public interface IReactive
{ 
    
    [UsedImplicitly]
    public void FinishAttack();

}

