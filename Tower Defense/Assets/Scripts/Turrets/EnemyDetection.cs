using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    const float RANGE = 5f; //provisional
    const float TIME_OFFSET_FOR_CHECKING_RANGE = 0.2f;

    Collider[] collidersCache = new Collider[32];
    LayerMask collisionMask;

    GameObject currentEnemy;
    bool isTargetingEnemy;

    Collider enemyCollider;

    float timer = 0;

    private void Start()
    {
        collisionMask = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        if(!isTargetingEnemy)
            detectEnemiesOnRangeAndSelectTheNearest();
        else
        {
            checkIfEnemyIsStillTargetableAndInRange();
            if (isTargetingEnemy)
                Debug.DrawLine(transform.position, currentEnemy.transform.position, Color.green);
            
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
        return Physics.OverlapSphereNonAlloc(this.transform.position, RANGE, collidersCache, collisionMask);
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
                currentEnemy = collidersCache[i].gameObject;
                enemyCollider = collidersCache[i];
            }
        }
    }

    void checkIfEnemyIsStillTargetableAndInRange()
    {
        if (!checkIfEnemyIsStillTargetable())
        {
            isTargetingEnemy = false;
            timer = 0;
        }
        
        //this part of code is called each a certain amount of time in order of increase the performance
        else if (timer > TIME_OFFSET_FOR_CHECKING_RANGE)
        {
            if (!checkIfEnemyIsStillInRange())
            {
                isTargetingEnemy = false;
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
        if (currentEnemy == null || !currentEnemy.activeSelf)
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, RANGE);
    }
}
