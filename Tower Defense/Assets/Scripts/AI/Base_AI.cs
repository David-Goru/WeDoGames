using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// <summary>
// Base class for AI enemies. It will trigger the initial behaviours and make the calls to the pathfinding system.
// </summary>

[SelectionBase]
public class Base_AI : Entity, ITurretDamage, IPooledObject, IStunnable, ISlowable, IFearable, IDamageable, IPoisonable, IKnockbackable, IDamageReductible
{
    [SerializeField] EnemyInfo info = null;

    [Header("Debug")]
    [SerializeField] Transform goal;
    [SerializeField] Transform currentTurret;
    [SerializeField] IEnemyDamageHandler currentTurretDamage;
    [SerializeField] LayerMask objectsLayer = new LayerMask();

    Animator anim;
    State currentState;

    Vector3[] path;
    Vector3 currentWaypoint;
    Vector3 pushDirection;

    int targetIndex;

    float speed;
    float rotationSpeed;
    float stunDuration;
    float pushDistance;
    float fearDuration;

    bool isStunned;
    bool isFeared;
    bool isKnockbacked;
    bool pathReached;
    bool pathSuccessful;

    public EnemyInfo Info { get => info; }
    public Transform Goal { get => goal; set => goal = value; }
    public Transform CurrentTurret { get => currentTurret; set => currentTurret = value; }
    public IEnemyDamageHandler CurrentTurretDamage { get => currentTurretDamage; set => currentTurretDamage = value; }
    public bool PathReached { get => pathReached; set => pathReached = value; }
    public bool PathSuccessful { get => pathSuccessful; set => pathSuccessful = value; }
    public float StunDuration { get => stunDuration; set => stunDuration = value; }
    public float PushDistance { get => pushDistance; set => pushDistance = value; }
    public Vector3 PushDirection { get => pushDirection; }
    public bool IsStunned { get => isStunned; set => isStunned = value; }
    public bool IsKnockbacked { get => isKnockbacked; set => isKnockbacked = value; }
    public float FearDuration { get => fearDuration; set => fearDuration = value; }
    public bool IsFeared { get => isFeared; set => isFeared = value; }

    public void OnObjectSpawn()
    {
        title = info.name;
        currentHP = Info.MaxHealth;
        maxHP = Info.MaxHealth;
        goal = Nexus.GetTransform;
        anim = transform.Find("Model").GetComponent<Animator>();
        isFeared = false;
        isStunned = false;
        speed = info.DefaultSpeed;
        rotationSpeed = info.InitRotationSpeed;
        currentState = new Move(this, anim, goal);
        currentTurret = null;

        EnemiesActive.Instance.enemiesList.Add(this);
    }

    public void EnemyUpdate()
    {
        currentState = currentState.Process();
        //print(currentState.GetType()); //Debug states

        if (isTargetTurretDead()) currentTurret = null;
    }

    void OnDisable()
    {
        StopCoroutine("FollowPath");
    }

    bool isTargetTurretDead()
    {
        return currentTurret != null && !currentTurret.gameObject.activeSelf;
    }

    bool checkDeath()
    {
        if (isEnemyDead())
        {
            killEnemy();
            return true;
        }
        return false;
    }

    bool isEnemyDead()
    {
        return currentHP <= 0 && gameObject.activeSelf;
    }

    void killEnemy()
    {
        StopAllCoroutines();
        ObjectPooler.GetInstance().ReturnToThePool(this.transform);
        Waves.KillEnemy();
        EnemiesActive.Instance.enemiesList.Remove(this);
    }

    public void checkPath() //Called if a new object is spawned. Checks if the path should be recalculated (i.e. a new turret is in your way)
    {
        if (!pathReached && path != null && path.Length > targetIndex)
        {
            Vector3 direction = path[targetIndex] - transform.position;         
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction.normalized, out hit, direction.magnitude, objectsLayer))
            {
                if (!hit.collider.CompareTag("Nexus")) //Make sure it's not the nexus what I'm detecting
                {
                    recalculatePath();
                }
            }
        }
    }

    private void recalculatePath() //Used by checkPath. Request another path for the enemy (calls pathfinding system).
    {
        PathData newTarget;

        if (currentTurret != null)
        {
            newTarget = new PathData(currentTurret.position, currentTurret);
        }
        else
        {
            newTarget = new PathData(Goal.position, Goal);
        }

        PathRequestManager.RequestPath(transform.position, newTarget, info.Range, OnPathFound);
    }

    public void OnTurretHit(Transform turretTransform, int damage, IEnemyDamageHandler enemyDamage)
    {
        currentHP -= damage;

        if (checkDeath()) return;
        if (currentTurret == null || currentState.Target == goal)
        {
            currentTurretDamage = enemyDamage;
            currentState.OnTurretHit(turretTransform);
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        StopCoroutine("FollowPath");

        this.pathSuccessful = pathSuccessful;
        path = newPath;
        targetIndex = 0;
        if (path.Length > 0 && gameObject.activeSelf) StartCoroutine("FollowPath");
        else pathReached = true;
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
            if(currentWaypoint != transform.position) transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(currentWaypoint - transform.position), rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void Stun(float secondsStunned)
    {
        stunDuration = secondsStunned;
        isStunned = true;

        StopCoroutine("FollowPath");

        if(currentTurret != null) currentState = new Stun(this, anim, currentTurret);
        else currentState = new Stun(this, anim, goal);
    }

    float baseSpeed = 0.0f;
    float baseRotationSpeed = 0.0f;
    Coroutine currentSlow = null;
    public void Slow(float secondsSlowed, float slowReduction)
    {
        if (!gameObject.activeSelf) return;

        if (currentSlow != null)
        {
            StopCoroutine(currentSlow);
            speed = baseSpeed;
            rotationSpeed = baseRotationSpeed;
        }
        currentSlow = StartCoroutine(slowEnemy(secondsSlowed, slowReduction));
    }

    IEnumerator slowEnemy(float secondsSlowed, float slowReduction)
    {
        baseSpeed = speed;
        baseRotationSpeed = rotationSpeed;

        speed -= slowReduction;
        rotationSpeed -= slowReduction;

        anim.SetFloat("animSpeed", 0.5f); //This could be slowReduction, but for now lets always leave it to half the speed for visual feedback

        yield return new WaitForSeconds(secondsSlowed);

        speed = baseSpeed;
        rotationSpeed = baseRotationSpeed;

        anim.SetFloat("animSpeed", 1.0f);
    }

    Coroutine currentPoison = null;
    public void Poison(float secondsPoisoned, int damagePerSecond)
    {
        if (!gameObject.activeSelf) return;

        if (currentPoison != null)
        {
            StopCoroutine(currentPoison);
            transform.Find("Model").Find("Color").GetComponent<Renderer>().material.color = new Color(1, 1, 1);
        }
        currentPoison = StartCoroutine(poisonEnemy(secondsPoisoned, damagePerSecond));
    }

    IEnumerator poisonEnemy(float secondsPoisoned, float damagePerSecond)
    {
        // Enable visual effects?
        Material material = transform.Find("Model").Find("Color").GetComponent<Renderer>().material;
        Color defaultColor = material.color;
        Color poisonColor = new Color(1, 0, 0);

        int timer = 0;
        material.color = poisonColor;
        while (timer < secondsPoisoned)
        {
            yield return new WaitForSeconds(1f);
            GetDamage(Mathf.RoundToInt(damagePerSecond));
            timer++;
        }
        material.color = defaultColor;

        // Disable visual effects?

        currentPoison = null;
    }

    public void Fear(float fearSeconds)
    {
        StartCoroutine(fearEnemy(fearSeconds)); //Fear also slows enemies

        fearDuration = fearSeconds;
        isFeared = true;

        StopCoroutine("FollowPath");

        if (currentTurret != null) currentState = new Fear(this, anim, currentTurret);
        else currentState = new Fear(this, anim, goal);
    }

    IEnumerator fearEnemy(float secondsFear)
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


    public void Knockback(float knockbackDistance, Vector3 knockbackDirection)
    {
        pushDistance = knockbackDistance;
        pushDirection = knockbackDirection;
        isKnockbacked = true;

        StopCoroutine("FollowPath");

        currentState = new Knockback(this, anim, goal);
    }

    public void ReduceDamage(float secondsDamageReduced, float damageReduction)
    {
        StartCoroutine(reduceDamageTimer(secondsDamageReduced, damageReduction));
    }

    IEnumerator reduceDamageTimer(float seconds, float damage)
    {
        int previousDamage = info.Damage;

        info.Damage -= (int)damage;

        yield return new WaitForSeconds(seconds);

        info.Damage = previousDamage;
    }

    public void GetDamage(int damage)
    {
        currentHP -= damage;

        checkDeath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TurretTrigger") && currentTurret == null)
        {
            currentTurretDamage = other.transform.parent.GetComponentInChildren<IEnemyDamageHandler>();
            currentTurret = other.transform.parent;
            currentState = new Move(this, anim, currentTurret);
        }
    }

    /* Editor */
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(path[i], Vector3.one / 3);

                if (i == targetIndex) Gizmos.DrawLine(transform.position, path[i]);
                else  Gizmos.DrawLine(path[i - 1], path[i]);
            }
        }
    }
}