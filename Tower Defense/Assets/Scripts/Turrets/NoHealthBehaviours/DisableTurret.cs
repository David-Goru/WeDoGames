using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class DisableTurret : MonoBehaviour, ITurretNoHealth
{
    public void OnTurretNoHealth()
    {
        Disable();
    }

    void Disable()
    {
        ObjectPooler.GetInstance().ReturnToThePool(transform);
    }
}