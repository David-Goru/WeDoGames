using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// <summary>
// Base class for AI enemies. It will trigger the initial behaviours and make the calls to the pathfinding system.
// </summary>

public class Base_AI : MonoBehaviour, ITurretDamage, IPooledObject, IStunnable, ISlowable, IFearable, IDamageable
{
    [SerializeField] float maxHealth = 0f;
    public float GetMaxHealth { get => maxHealth; }

    [SerializeField] float damage = 0f;
    public float Damage { get => damage; }

    [SerializeField] float attackSpeed = 0f;
    public float AttackSpeed { get => attackSpeed; }

    [SerializeField] float range = 0f;
    public float Range { get => range; }

    private float health;
    public float GetHealth { get => health; }

    private Animator anim;
    private State currentState;

    [HideInInspector] public BuildingRange Goal;
    [HideInInspector] public Transform currentTurret;
    public IEnemyDamageHandler currentTurretDamage;

    //A*

    private float speed;
    [SerializeField] float initSpeed;
    public float GetSpeed { get => initSpeed; }

    private float rotationSpeed;
    [SerializeField] float initRotationSpeed;
    private Vector3[] path;
    private int targetIndex;
    [HideInInspector] public bool pathReached;

    private Vector3 currentWaypoint;

    [HideInInspector] public float stunDuration;
    [HideInInspector] public bool isStunned;

    [HideInInspector] public float fearDuration;
    [HideInInspector] public bool isFeared;
    [HideInInspector] public Vector3 fleePos;

    public void OnObjectSpawn()
    {
        Goal = GameObject.FindGameObjectWithTag("Nexus").GetComponent<BuildingRange>();
        health = maxHealth;
        anim = GetComponent<Animator>();
        isFeared = false;
        isStunned = false;
        speed = initSpeed;
        rotationSpeed = initRotationSpeed;
        currentState = new Move(this, anim, Goal);
        currentTurret = null;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game") return;
        Goal = GameObject.FindGameObjectWithTag("Nexus").GetComponent<BuildingRange>();
        health = maxHealth;
        anim = GetComponent<Animator>();
        currentState = new Move(this, anim, Goal);
        currentTurret = null;
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();

        if (currentTurret != null && !currentTurret.gameObject.activeSelf)
        {
            currentTurret = null;
        }
    }

    void checkDeath()
    {
        if(health <= 0)
        {
            if (gameObject.activeSelf)
            {
                ObjectPooler.GetInstance().ReturnToThePool(this.transform);
                WavesHandler.EnemyKilled();
            }
        }
    }

    public void OnTurretHit(BuildingRange turretTransform, float damage, IEnemyDamageHandler enemyDamage)
    {
        health -= damage;
        currentTurretDamage = enemyDamage;
        if (currentTurret == null)
        {
            currentTurret = turretTransform.transform;
            currentState.OnTurretHit(turretTransform, damage, enemyDamage);
        }
        checkDeath();
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            StopCoroutine("FollowPath"); //This is for stopping the coroutine in case it's already running
            path = newPath;
            targetIndex = 0;
            if (path.Length > 0 && this.gameObject.activeSelf && !isStunned && !isFeared) //If the AI is stunned or feared, we will say that it reached the end of the path (it will forget it's target)
            {
                StartCoroutine("FollowPath");
            }
            else
            {
                pathReached = true;
            }
        }
    }

    IEnumerator FollowPath()
    {
        currentWaypoint = path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length) //Path finished
                {
                    pathReached = true;
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            if(currentWaypoint != transform.position)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(currentWaypoint - transform.position), rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDisable()
    {
        StopCoroutine("FollowPath");
    }

    public void Flee()
    {
        transform.position = Vector3.MoveTowards(transform.position, fleePos, speed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(fleePos), rotationSpeed * Time.deltaTime);
    }

    //For visual hint
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(path[i], Vector3.one / 3);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

    public void Stun(float secondsStunned)
    {
        stunDuration = secondsStunned;
        isStunned = true;

        StopCoroutine("FollowPath");

        currentState = new Stun(this, anim, Goal);
    }

    public void Slow(float secondsSlowed)
    {
        StartCoroutine(slowEnemy(secondsSlowed));
    }

    private IEnumerator slowEnemy(float secondsSlowed)
    {
        //Maybe we should also pass the slow values
        speed /= 2f;
        rotationSpeed /= 2f;

        anim.SetFloat("animSpeed", 0.5f);

        yield return new WaitForSeconds(secondsSlowed);

        speed *= 2f;
        rotationSpeed *= 2f;

        anim.SetFloat("animSpeed", 1f);
    }

    public void Fear(float fearSeconds)
    {
        StartCoroutine(fearEnemy(fearSeconds)); //Fear also slows enemies

        fearDuration = fearSeconds;
        isFeared = true;

        fleePos = -transform.forward * 20f;

        StopCoroutine("FollowPath");

        currentState = new Fear(this, anim, Goal);
    }

    private IEnumerator fearEnemy(float secondsFear)
    {
        //Maybe we should also pass the slow values
        speed /= 2f;
        rotationSpeed *= 2f;

        anim.SetFloat("animSpeed", 0.5f);

        yield return new WaitForSeconds(secondsFear);

        speed *= 2f;
        rotationSpeed /= 2f;

        anim.SetFloat("animSpeed", 1f);
    }

    public void GetDamage(float damage)
    {
        health -= damage;

        checkDeath();
    }
}
