using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private float _jumpCooldown = 0.2f;

    private float _lastJumpTime;
    private Rigidbody2D _rb;
    private Collider2D _playerCollider;
    private bool _isGrounded;
    private Collider2D[] _ignoredPlatforms = new Collider2D[10]; // Кэш для игнорируемых платформ

    public bool IsGrounded => _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<Collider2D>();
    }

    public void Move(Vector2 direction)
    {
        if (direction.magnitude > 1f)
            direction.Normalize();

        _rb.velocity = new Vector2(direction.x * _moveSpeed, _rb.velocity.y);
    }

    public void Jump()
    {
        if (!_isGrounded || Time.time - _lastJumpTime < _jumpCooldown)
            return;

        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        _lastJumpTime = Time.time;
    }

    public void FallThrough()
    {
        if (!_isGrounded) return;

        // Находим все платформы под игроком
        int platformsCount = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            0.6f,
            _ignoredPlatforms,
            _groundLayer
        );

        // Временно игнорируем их
        for (int i = 0; i < platformsCount; i++)
        {
            Physics2D.IgnoreCollision(_playerCollider, _ignoredPlatforms[i], true);
        }

        // Включаем обратно через 0.5 секунды
        Invoke(nameof(ResetIgnoredPlatforms), 0.5f);
    }

    private void ResetIgnoredPlatforms()
    {
        foreach (var platform in _ignoredPlatforms)
        {
            if (platform != null)
                Physics2D.IgnoreCollision(_playerCollider, platform, false);
        }
    }

    private void FixedUpdate()
    {
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            _playerCollider.bounds.center,
            new Vector2(_playerCollider.bounds.size.x * 0.9f, _playerCollider.bounds.size.y),
            0f,
            Vector2.down,
            _groundCheckDistance,
            _groundLayer
        );

        _isGrounded = hit.collider != null;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.6f);
    }
#endif
}