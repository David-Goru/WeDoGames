public class DisableAreaObject : TurretNoHealth
{
    IRangeViewable rangeViewable;

    void Awake()
    {
        rangeViewable = transform.parent.GetComponentInChildren<IRangeViewable>();
    }

    public override void OnTurretNoHealth()
    {
        if (rangeViewable != null) rangeViewable.HideRange();
    }
}