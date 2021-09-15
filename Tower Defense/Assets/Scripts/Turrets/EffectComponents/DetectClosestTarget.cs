using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class detects the first enemy on enter the range. When enemy dies, take the closest one and mantains it until he dies.
/// </summary>
public class DetectClosestTarget : CurrentTargetsOnRange
{
    const float TIME_OFFSET_FOR_CHECKING_RANGE = 0.2f;

    [SerializeField] LayerMask targetLayer = 0;
    ITargetsDetector targetsDetector;
    protected TurretStats turretStats;
    protected List<Transform> currentTargets = new List<Transform>();
    protected BaseAI currentEnemyAI;

    float timer = 0;

    public override List<Transform> CurrentTargets { get { return currentTargets; } }

    public override void InitializeComponent()
    {
        getDependencies();
        initializeMembers();
    }

    public override void UpdateComponent()
    {
        UpdateTarget();
    }

    private void getDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
        targetsDetector = GetComponent<ITargetsDetector>();
    }

    private void initializeMembers()
    {
        areTargetsInRange = false;
        timer = 0f;
        currentTargets.Clear();
        currentEnemyAI = null;
    }

    void UpdateTarget()
    {
        if (currentEnemyAI == null) detectEnemiesOnRangeAndSelectTheNearest();
        else checkIfEnemyIsStillTargetableAndInRange();

        ////////////////////////////DEBUG////////////////////////////////////
        if (currentEnemyAI != null) Debug.DrawLine(transform.position, currentTargets[0].transform.position, Color.green);
        ////////////////////////////DEBUG////////////////////////////////////
    }

    void detectEnemiesOnRangeAndSelectTheNearest()
    {
        float range = turretStats.GetStatValue(StatType.ATTACKRANGE);
        List<Transform> targetsOnRange = detectTargets(range);
        targetsOnRange.RemoveAll((Transform enemy) => TurretUtilities.IsEnemyDying(enemy));
        if (targetsOnRange.Count > 0)
        {
            areTargetsInRange = true;
            selectTheNearestEnemy(targetsOnRange);
        }
        else areTargetsInRange = false;
    }
    
    protected List<Transform> detectTargets(float range)
    {
        return targetsDetector.GetTargets(range, targetLayer);
    }

    protected virtual void selectTheNearestEnemy(List<Transform> targetsOnRange)
    {
        float minDistanceToTurret = Mathf.Infinity;
        int nTargets = targetsOnRange.Count;
        for (int i = 0; i < nTargets; i++)
        {
            float newDistanceToTurret = Vector3.Distance(transform.position, targetsOnRange[i].transform.position);
            if (newDistanceToTurret < minDistanceToTurret)
            {
                minDistanceToTurret = newDistanceToTurret;
                if (currentTargets.Count > 0) currentTargets[0] = targetsOnRange[i];
                else currentTargets.Add(targetsOnRange[i]);
            }
        }
        currentEnemyAI = currentTargets[0].GetComponent<BaseAI>();
    }

    void checkIfEnemyIsStillTargetableAndInRange()
    {
        if (!isEnemyStillTargetable())
        {
            deleteCurrentEnemy();
            timer = 0;
        }

        // this part of the code is only called from time to time to increase performance
        else if (timer > TIME_OFFSET_FOR_CHECKING_RANGE)
        {
            if (!isEnemyStillInRange()) deleteCurrentEnemy();
            timer = 0;
        }
        else timer += Time.deltaTime;
    }

    bool isEnemyStillTargetable()
    {
        return (currentTargets[0] != null && currentTargets[0].gameObject.activeSelf && !currentEnemyAI.IsDying);
    }

    protected virtual bool isEnemyStillInRange()
    {
        float range = turretStats.GetStatValue(StatType.ATTACKRANGE);

        List<Transform> targets = detectTargets(range);
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == currentTargets[0]) return true;
        }
        return false;
    }

    void deleteCurrentEnemy()
    {
        currentTargets.Clear();
        currentEnemyAI = null;
    }

    ////////////////////////////DEBUG////////////////////////////////////
    [SerializeField] float debugRange = 2f;
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (turretStats != null) debugRange = turretStats.GetStatValue(StatType.ATTACKRANGE);
        Gizmos.DrawWireSphere(this.transform.position, debugRange);
    }
    ////////////////////////////DEBUG////////////////////////////////////
}