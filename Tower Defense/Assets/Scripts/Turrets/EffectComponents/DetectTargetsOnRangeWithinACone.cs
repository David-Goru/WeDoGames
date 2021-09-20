using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DetectTargetsOnRangeWithinACone : CurrentTargetsOnRange
{
    ITargetsDetector targetsDetector;
    [SerializeField] LayerMask targetLayer = 0;
    [SerializeField] Transform centerPointPivot = null;

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
        List<Transform> targets = targetsDetector.GetTargets(range, targetLayer);
        areTargetsInRange = targets.Count > 0;
        selectTargetsWithinTheCone(targets);
        return targets;
    }

    void selectTargetsWithinTheCone(List<Transform> targets)
    {
        float angle = turretStats.GetStatValue(StatType.CONEANGLE)/2;
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            Vector3 dirToEnemy = targets[i].position - centerPointPivot.position;
            dirToEnemy.y = 0f;
            if (!isWithinCone(dirToEnemy, angle)) targets.Remove(targets[i]);
        }
    }

    bool isWithinCone(Vector3 dirToEnemy, float angle)
    {
        return Vector3.Angle(centerPointPivot.forward, dirToEnemy) <= angle;
    }

#if UNITY_EDITOR
    ////////////////////////////DEBUG////////////////////////////////////
    [SerializeField] float debugRange = 2f;
    [SerializeField] float debugAngle = 45f;
    [SerializeField] Color coneColor = Color.red;
    void OnDrawGizmos()
    {
        Handles.color = coneColor;
        if (turretStats != null)
        {
            debugRange = turretStats.GetStatValue(StatType.ATTACKRANGE);
            debugAngle = turretStats.GetStatValue(StatType.CONEANGLE);
        }
        Vector3 fromVector = Quaternion.Euler(0, -debugAngle/2, 0) * centerPointPivot.forward;
        Handles.DrawSolidArc(centerPointPivot.position, Vector3.up, fromVector, debugAngle, debugRange);
    }
    ////////////////////////////DEBUG////////////////////////////////////
#endif
}