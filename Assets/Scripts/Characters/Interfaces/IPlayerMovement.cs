using UnityEngine;

public interface IPlayerMovement
{
    void Move(Vector2 direction);
    void Jump();
    void FallThrough(); // Новый метод
    bool IsGrounded { get; }
}