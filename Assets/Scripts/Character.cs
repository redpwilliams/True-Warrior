using UnityEngine;

public class Character : MonoBehaviour
{
    // Components
    protected Rigidbody2D Rb2d;
    protected Animator Anim;
    
    // Running Animation
    private static readonly int Running = Animator.StringToHash("ShouldRun");
    [SerializeField] private float _finalPosition;
    [SerializeField] private float _speed = 2f;
    private bool _isRunning = true;
    
    // Attacking Animation
    protected static readonly int Attacking = Animator.StringToHash
    ("ShouldAttack");

    private void Awake()
    {
        Rb2d = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }
    

    protected virtual void Start()
    {
        // Start Character movement
        Anim.SetBool(Running, _isRunning);
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
        if (Rb2d.position.x < _finalPosition) return;
        
        // Stop the movement (set velocity to zero)
        Rb2d.velocity = Vector2.zero;
        _isRunning = false;
        Anim.SetBool(Running, _isRunning);
    }
}