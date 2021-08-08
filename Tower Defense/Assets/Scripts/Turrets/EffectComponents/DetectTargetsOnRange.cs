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
    List<Transform> currentTargets = new List<Transform>();

    public override List<Transform> CurrentTargets { get => currentTargets; }

    public override void InitializeComponent()
    {
        turretStats = GetComponentInParent<TurretStats>();
        targetsDetector = GetComponent<ITargetsDetector>();
        currentTargets.Clear();
        areTargetsInRange = false;
    }

    public override void UpdateComponent()
    {
        getTargets();
    }

    void getTargets()
    {
        float range = turretStats.GetStatValue(StatType.ATTACKRANGE);
        currentTargets = targetsDetector.GetTargets(range, targetLayer);
        areTargetsInRange = currentTargets.Count > 0;
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
