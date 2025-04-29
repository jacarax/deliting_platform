using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private IPlayerMovement _movement;

    private void Awake()
    {
        _movement = GetComponent<IPlayerMovement>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleFallThrough();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        _movement.Move(new Vector2(horizontal, 0));
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _movement.Jump();
        }
    }

    private void HandleFallThrough()
    {
        if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            _movement.FallThrough();
        }
    }
}