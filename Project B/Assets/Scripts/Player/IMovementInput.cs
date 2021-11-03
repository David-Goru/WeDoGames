using UnityEngine;

public interface IMovementInput
{
    public bool MoveInputRecieved();
    public Vector3 HorizontalDirection();
    public Vector3 VerticalDirection();
}