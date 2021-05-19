using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// <summary>
// Base class for AI enemies. It will trigger the initial behaviours and make the calls to the pathfinding system.
// </summary>

[SelectionBase]
public class Base_AI : Entity, ITurretDamage, IPooledObject, IStunnable, ISlowable, IFearable, IDamageable
{
    [SerializeField] float damage = 0f;
    [SerializeField] float attackSpeed = 0f;
    [SerializeField] float range = 0f;
    [SerializeField] float defaultSpeed = 5f;

    [Header("Other stuff")]
    [SerializeField] float initRotationSpeed = 300f;

    Animator anim;
    State currentState;

    public float Damage { get => damage; set => damage = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float Range { get => range; set => range = value; }
    [HideInInspector] public Transform Goal;
    [HideInInspector] public Transform currentTurret;
    public IEnemyDamageHandler currentTurretDamage;

    //A*

    private float speed;
    private float rotationSpeed;
    private Vector3[] path;
    private int targetIndex;
    [HideInInspector] public bool pathReached;

    private Vector3 currentWaypoint;

    [HideInInspector] public float stunDuration;
    [HideInInspector] public bool isStunned;
    [HideInInspector] public float fearDuration;
    [HideInInspector] public bool isFeared;

    public void OnObjectSpawn()
    {
        Goal = Nexus.GetTransform;
        currentHP = maxHP;
        anim = transform.Find("Model").GetComponent<Animator>();
        isFeared = false;
        isStunned = false;
        speed = defaultSpeed;
        rotationSpeed = initRotationSpeed;
        currentState = new Move(this, anim, Goal);
        currentTurret = null;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game") return;
        Goal = Nexus.GetTransform;
        currentHP = maxHP;
        anim = transform.Find("Model").GetComponent<Animator>();
        currentState = new Move(this, anim, Goal);
        currentTurret = null;
    }

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
        if (currentHP <= 0)
        {
            if (gameObject.activeSelf)
            {
                ObjectPooler.GetInstance().ReturnToThePool(this.transform);
                WavesHandler.EnemyKilled();
            }
        }
    }

    public void OnTurretHit(Transform turretTransform, float damage, IEnemyDamageHandler enemyDamage)
    {
        currentHP -= Mathf.RoundToInt(damage);
        currentTurretDamage = enemyDamage;
        if (currentTurret == null || currentState.Target == Goal)
        {
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
            if (path.Length > 0 && this.gameObject.activeSelf) //If the AI is stunned or feared, we will say that it reached the end of the path (it will forget it's target)
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
        currentHP -= Mathf.RoundToInt(damage);

        checkDeath();
    }

    /* UI */
    const string info = @"Attack Range: {0:0.##}
Attack Rate: {1:0.##}
Attack Damage: {2:0.##}
Speed: {3:0.##}";

    public override string GetExtraInfo()
    {
        return string.Format(info, range, attackSpeed, damage, speed);
    }
}