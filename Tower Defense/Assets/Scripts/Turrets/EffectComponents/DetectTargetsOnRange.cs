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
    }

    public override void UpdateComponent()
    {
    }

    List<Transform> getTargets()
    {
        float range = turretStats.GetStatValue(StatType.ATTACKRANGE);
        return targetsDetector.GetTargets(range, targetLayer);
    }

    [SerializeField] float debugRange = 2f;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (turretStats != null)
            debugRange = turretStats.GetStatValue(StatType.ATTACKRANGE);
        Gizmos.DrawWireSphere(this.transform.position, debugRange);
    }
}