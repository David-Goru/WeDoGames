using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    const float TIME_OFFSET_FOR_CHECKING_RANGE = 0.2f;

    Collider[] collidersCache = new Collider[32];
    LayerMask collisionMask;

    Transform currentEnemy;
    Collider enemyCollider;
    
    bool isTargetingEnemy;

    float timer = 0;
    float range;

    private void Start()
    {
        collisionMask = LayerMask.GetMask("Enemy");
    }

    public void SetRange(float range)
    {
        this.range = range;
    }

    public Transform UpdateTarget()
    {
        if (!isTargetingEnemy)
            detectEnemiesOnRangeAndSelectTheNearest();
        else
        {
            checkIfEnemyIsStillTargetableAndInRange();
            if (isTargetingEnemy)
                Debug.DrawLine(transform.position, currentEnemy.transform.position, Color.green);

        }
        return currentEnemy;
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
        return Physics.OverlapSphereNonAlloc(this.transform.position, range, collidersCache, collisionMask);
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
                currentEnemy = collidersCache[i].transform;
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
        if (currentEnemy == null || !currentEnemy.gameObject.activeSelf)
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
        currentEnemy = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, range);
    }
}
