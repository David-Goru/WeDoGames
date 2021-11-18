using UnityEngine;

public interface IMovementInput
{
    public bool MoveInputReceived();
    public Vector3 HorizontalDirection();
    public Vector3 VerticalDirection();
}