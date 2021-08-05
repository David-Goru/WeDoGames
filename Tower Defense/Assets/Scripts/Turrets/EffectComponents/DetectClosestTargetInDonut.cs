﻿using System.Collections.Generic;
using UnityEngine;

public class DetectClosestTargetInDonut : DetectClosestTarget
{
    protected override void selectTheNearestEnemy(List<Transform> targetsOnRange)
    {
        float minDistanceToTurret = Mathf.Infinity;
        float minimumRange = turretStats.GetStatValue(StatType.MINIMUMRANGE);
        int nTargets = targetsOnRange.Count;
        for (int i = 0; i < nTargets; i++)
        {
            float newDistanceToTurret = Vector3.Distance(transform.position, targetsOnRange[i].transform.position);
            if (newDistanceToTurret <= minimumRange) continue;
            if (newDistanceToTurret < minDistanceToTurret)
            {
                minDistanceToTurret = newDistanceToTurret;
                if (currentTargets.Count > 0) currentTargets[0] = targetsOnRange[i];
                else currentTargets.Add(targetsOnRange[i]);
            }
        }
        if (minDistanceToTurret == Mathf.Infinity) areTargetsInRange = false;
    }

    protected override bool isEnemyStillInRange()
    {
        float minimumRange = turretStats.GetStatValue(StatType.MINIMUMRANGE);
        if (Vector3.Distance(transform.position, currentTargets[0].position) <= minimumRange) return false;
        return base.isEnemyStillInRange();
    }

    ////////////////////////////DEBUG////////////////////////////////////
    [SerializeField] float debugMinimumRange = 1f;
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        if (turretStats != null) debugMinimumRange = turretStats.GetStatValue(StatType.MINIMUMRANGE);
        Gizmos.DrawWireSphere(this.transform.position, debugMinimumRange);
    }
    ////////////////////////////DEBUG////////////////////////////////////
}
