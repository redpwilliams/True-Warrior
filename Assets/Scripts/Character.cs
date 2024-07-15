using UnityEngine;

public class Character : MonoBehaviour
{
    // Components
    protected Rigidbody2D Rb2d;
    private Animator _animator;
    
    // Running Animation
    private static readonly int Running = Animator.StringToHash("ShouldRun");
    [SerializeField] private float _finalPosition;
    [SerializeField] private float _speed = 1f;
    private bool _isRunning = true;
    
    // Attacking Animation
    protected static readonly int Attacking = Animator.StringToHash
    ("ShouldAttack");

    private void Awake()
    {
        Rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    

    private void Start()
    {
        // Start Character movement
        _animator.SetBool(Running, _isRunning);
            EventManager.Events.OnBeginAttack += Test;
    }

    private static void Test()
    {
        Debug.Log(("test"));
    }

    private void FixedUpdate()
    {
        // Disregard if not moving
        if (!_isRunning)
        {
            EventManager.Events.BeginAttack();
            return;
        }
        
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
        _animator.SetBool(Running, _isRunning);
    }
}