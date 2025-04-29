using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LinearMovement : MonoBehaviour, IMovable
{
    [Header("Movement Settings")]
    [SerializeField, Range(0.1f, 10f)] private float _speed = 2f;
    [SerializeField] private bool _useSmoothAcceleration = true;
    [SerializeField, Tooltip("Only if smooth acceleration enabled")]
    private float _accelerationTime = 0.5f;

    private Rigidbody2D _rb;
    private float y;
    private float _currentVelocity;
    private float _velocitySmoothing;
    private bool _shouldMove;

    public float Speed
    {
        get => _speed;
        set => _speed = Mathf.Clamp(value, 0.1f, 100f);
    }

    public bool IsMoving { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0; // Отключаем гравитацию
        y = _rb.velocity.y;
    }

    public void StartMovement()
    {
        _shouldMove = true;
        IsMoving = true;
    }

    public void StopMovement()
    {
        _shouldMove = false;
        IsMoving = false;
    }

    private void FixedUpdate()
    {
        if (!_shouldMove)
        {
            if (_useSmoothAcceleration)
                ApplySmoothStop();
            return;
        }

        MoveRight();
    }

    private void MoveRight()
    {
        float targetVelocity = _speed;

        if (_useSmoothAcceleration)
        {
            _currentVelocity = Mathf.SmoothDamp(
                _currentVelocity,
                targetVelocity,
                ref _velocitySmoothing,
                _accelerationTime
            );
        }
        else
        {
            _currentVelocity = targetVelocity;
        }

        _rb.velocity = new Vector2(_currentVelocity, y);
    }

    private void ApplySmoothStop()
    {
        _currentVelocity = Mathf.SmoothDamp(
            _currentVelocity,
            0f,
            ref _velocitySmoothing,
            _accelerationTime
        );

        _rb.velocity = new Vector2(_currentVelocity, y);
    }

}