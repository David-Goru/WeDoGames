using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class DetectTargetsOnRange : EffectComponent, ICurrentTargetsOnRange
{
    ITargetsDetector targetsDetector;
    TurretStats turretStats;
    [SerializeField] LayerMask targetLayer = 0;

    public List<Transform> CurrentTargets { get { return getTargets(); } private set { } }

    public override void InitializeComponent()
    {
        turretStats = GetComponentInParent<TurretStats>();
        targetsDetector = GetComponent<ITargetsDetector>();
        targetsDetector.TargetLayer = targetLayer;
    }

    public override void UpdateComponent()
    {
    }

    List<Transform> getTargets()
    {
        targetsDetector.Range = turretStats.GetStatValue(StatType.ATTACKRANGE);
        return targetsDetector.GetTargets();
    }

    [SerializeField] float debugRange = 2f;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, debugRange);
    }
}