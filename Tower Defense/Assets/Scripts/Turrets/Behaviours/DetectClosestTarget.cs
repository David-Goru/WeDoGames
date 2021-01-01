using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class detects the first enemy on enter the range. When enemy dies, take the closest one and mantains it until he dies.
/// </summary>
public class DetectClosestTarget : MonoBehaviour, ITurretBehaviour, ICurrentTargetsOnRange
{
    const float TIME_OFFSET_FOR_CHECKING_RANGE = 0.2f;

    ITargetsDetector targetsDetector;

    List<Transform> currentTargets = new List<Transform>();
    
    bool isTargetingEnemy;

    float timer = 0;

    TurretStats turretStats;

    public List<Transform> CurrentTargets { get { return currentTargets; } private set { } }


    public void InitializeBehaviour()
    {
        GetDependencies();
    }

    private void GetDependencies()
    {
        turretStats = GetComponent<TurretStats>();
        targetsDetector = GetComponent<ITargetsDetector>();
        targetsDetector.TargetLayer = LayerMask.GetMask("Enemy");
    }

    public void UpdateBehaviour()
    {
        UpdateTarget();
    }

    void UpdateTarget()
    {
        targetsDetector.Range = turretStats.AttackRange;
        if (!isTargetingEnemy)
            detectEnemiesOnRangeAndSelectTheNearest();
        else
        {
            checkIfEnemyIsStillTargetableAndInRange();
            if (isTargetingEnemy)
                Debug.DrawLine(transform.position, currentTargets[0].transform.position, Color.green);

        }
    }

    void detectEnemiesOnRangeAndSelectTheNearest()
    {
        List<Transform> targetsOnRange = detectTargets();
        if (targetsOnRange.Count > 0)
        {
            isTargetingEnemy = true;
            selectTheNearestEnemy(targetsOnRange);
        }
    }
    
    List<Transform> detectTargets()
    {
        return targetsDetector.GetTargets();
    }

    void selectTheNearestEnemy(List<Transform> targetsOnRange)
    {
        float minDistanceToTurret = Mathf.Infinity;
        int listCount = targetsOnRange.Count;
        for (int i = 0; i < listCount; i++)
        {
            float newDistanceToTurret = Vector3.Distance(transform.position, targetsOnRange[i].transform.position);
            if (newDistanceToTurret < minDistanceToTurret)
            {
                minDistanceToTurret = newDistanceToTurret;
                if (currentTargets.Count > 0)
                    currentTargets[0] = targetsOnRange[i];
                else
                    currentTargets.Add(targetsOnRange[i]);
            }
        }
    }

    void checkIfEnemyIsStillTargetableAndInRange()
    {
        if (!checkIfEnemyIsStillTargetable())
        {
            deleteCurrentEnemy();
            timer = 0;
        }
        
        //this part of code is called each a certain amount of time in order to increase the performance
        else if (timer > TIME_OFFSET_FOR_CHECKING_RANGE)
        {
            if (!checkIfEnemyIsStillInRange())
            {
                deleteCurrentEnemy();
            }
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    bool checkIfEnemyIsStillTargetable()
    {
        if (currentTargets[0] == null || !currentTargets[0].gameObject.activeSelf)
            return false;
        return true;
    }

    bool checkIfEnemyIsStillInRange()
    {
        List<Transform> targets = detectTargets();

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == currentTargets[0])
                return true;
        }
        return false;
    }

    void deleteCurrentEnemy()
    {
        isTargetingEnemy = false;
        currentTargets.Clear();
    }

    [SerializeField] float range;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, range);
    }

}
