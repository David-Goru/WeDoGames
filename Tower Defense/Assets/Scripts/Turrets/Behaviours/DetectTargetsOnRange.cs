using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class DetectTargetsOnRange : MonoBehaviour, ICurrentTargetsOnRange
{
    ITargetsDetector targetsDetector;
    TurretStats turretStats;

    public List<Transform> CurrentTargets { get { return getTargets(); } private set { } }

    void Awake()
    {
        turretStats = GetComponent<TurretStats>();
        targetsDetector = GetComponent<ITargetsDetector>();
        targetsDetector.TargetLayer = LayerMask.GetMask("Enemy");
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