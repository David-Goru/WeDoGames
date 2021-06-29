using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class DetectTargetsOnRange : CurrentTargetsOnRange
{
    [SerializeField] LayerMask targetLayer = 0;
    ITargetsDetector targetsDetector;
    TurretStats turretStats;

    public override List<Transform> CurrentTargets { get { return getTargets(); } }

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

    ////////////////////////////DEBUG////////////////////////////////////
    [SerializeField] float debugRange = 2f;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (turretStats != null) debugRange = turretStats.GetStatValue(StatType.ATTACKRANGE);
        Gizmos.DrawWireSphere(this.transform.position, debugRange);
    }
    ////////////////////////////DEBUG////////////////////////////////////
}
