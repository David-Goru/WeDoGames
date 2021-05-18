/// <summary>
/// This is a class
/// </summary>
public class DisableTurret : TurretNoHealth
{
    public override void OnTurretNoHealth()
    {
        Disable();
    }

    void Disable()
    {
        ObjectPooler.GetInstance().ReturnToThePool(transform);
    }
}
