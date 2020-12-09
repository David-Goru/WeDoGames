using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class Rotate : MonoBehaviour, ITurretBehaviour
{

    public void InitializeBehaviour()
    {
        
    }

    public void UpdateBehaviour()
    {
        transform.Rotate(new Vector3(0, 10f, 0));
    }
}