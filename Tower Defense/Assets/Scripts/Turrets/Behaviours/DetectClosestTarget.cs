using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class detects the first enemy on enter the range. When enemy dies, take the closest one and mantains it until he dies.
/// </summary>
public class DetectClosestTarget : MonoBehaviour, ITurretBehaviour, ICurrentTargetsOnRange
{
    const float TIME_OFFSET_FOR_CHECKING_RANGE = 0.2f;

    Collider[] collidersCache = new Collider[64];
    LayerMask collisionMask;

    Transform[] currentEnemies = new Transform[1];
    Collider enemyCollider;
    
    bool isTargetingEnemy;

    float timer = 0;

    TurretStats turretStats;

    public Transform[] CurrentTargets { get { return currentEnemies; } private set { } }

    private void Start()
    {
        collisionMask = LayerMask.GetMask("Enemy");
    }

    public void InitializeBehaviour()
    {
        turretStats = transform.root.GetComponentInChildren<TurretStats>();
    }

    public void UpdateBehaviour()
    {
        UpdateTarget();
    }

    void UpdateTarget()
    {
        if (!isTargetingEnemy)
            detectEnemiesOnRangeAndSelectTheNearest();
        else
        {
            checkIfEnemyIsStillTargetableAndInRange();
            if (isTargetingEnemy)
                Debug.DrawLine(transform.position, currentEnemies[0].transform.position, Color.green);

        }
    }

    void detectEnemiesOnRangeAndSelectTheNearest()
    {
        int enemiesOnRange = detectEnemies();
        if (enemiesOnRange > 0)
        {
            isTargetingEnemy = true;
            selectTheNearestEnemy(enemiesOnRange);
        }
    }
    
    int detectEnemies()
    {
        return Physics.OverlapSphereNonAlloc(this.transform.position, turretStats.AttackRange, collidersCache, collisionMask);
    }

    void selectTheNearestEnemy(int enemiesOnRange)
    {
        float minDistanceToTurret = Mathf.Infinity;
        for (int i = 0; i < enemiesOnRange; i++)
        {
            float newDistanceToTurret = Vector3.Distance(transform.position, collidersCache[i].transform.position);
            if (newDistanceToTurret < minDistanceToTurret)
            {
                minDistanceToTurret = newDistanceToTurret;
                currentEnemies[0] = collidersCache[i].transform;
                enemyCollider = collidersCache[i];
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
        if (currentEnemies[0] == null || !currentEnemies[0].gameObject.activeSelf)
            return false;
        return true;
    }

    bool checkIfEnemyIsStillInRange()
    {
        int enemiesOnRange = detectEnemies();

        for (int i = 0; i < enemiesOnRange; i++)
        {
            if (collidersCache[i] == enemyCollider)
                return true;
        }
        return false;
    }

    void deleteCurrentEnemy()
    {
        isTargetingEnemy = false;
        currentEnemies[0] = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, turretStats.AttackRange);
    }

}
