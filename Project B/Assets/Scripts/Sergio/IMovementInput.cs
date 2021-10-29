using UnityEngine;

public interface IMovementInput
{
    public bool moveInputRecieved();
    public Vector3 horizontalDirection();
    public Vector3 verticalDirection();
}
