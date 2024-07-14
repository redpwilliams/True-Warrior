using UnityEngine;

public class Character : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    private Animator _animator;
    [SerializeField] private float _finalPosition = 0f;
    [SerializeField] private float _speed = 1f;
    private bool _isRunning = true;

    private static readonly int Running = Animator.StringToHash("ShouldRun");

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Start Character movement
        _animator.SetBool(Running, _isRunning);
    }

    private void FixedUpdate()
    {
        // Disregard if not moving
        if (!_isRunning) return;
        
        // Move the character towards the final position
        Vector2 currentPosition = _rb2d.position;
        Vector2 targetPosition = new Vector2(_finalPosition, currentPosition.y);
        _rb2d.MovePosition(Vector2.MoveTowards(currentPosition, targetPosition,
            _speed * Time.fixedDeltaTime));

        // Check if the character has reached or passed the final position
        if (_rb2d.position.x < _finalPosition) return;
        
        // Stop the movement (set velocity to zero)
        _rb2d.velocity = Vector2.zero;
        _isRunning = false;
        _animator.SetBool(Running, _isRunning);
    }
}