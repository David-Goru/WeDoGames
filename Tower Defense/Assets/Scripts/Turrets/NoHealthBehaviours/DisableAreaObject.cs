public class DisableAreaObject : TurretNoHealth
{
    IRangeViewable rangeViewable;

    void Awake()
    {
        rangeViewable = transform.parent.GetComponentInChildren<IRangeViewable>();
    }

    public override void OnTurretNoHealth()
    {
        rangeViewable.HideRange();
    }
}