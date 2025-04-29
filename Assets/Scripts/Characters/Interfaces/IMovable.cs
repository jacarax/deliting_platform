

public interface IMovable
{
    float Speed { get; set; }
    bool IsMoving { get; }
    void StartMovement();
    void StopMovement();
}