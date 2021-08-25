using System.Collections;
using UnityEngine;

[SelectionBase]
public class BaseAI : Entity, ITurretDamage, IPooledObject, IStunnable, ISlowable, IFearable, IDamageable, IPoisonable, IKnockbackable, IDamageReductible
{
    [SerializeField] EnemyInfo info = null;
    [SerializeField] Color poisonedColor = Color.magenta;

    [Header("Debug")]
    [SerializeField] protected Transform goal;
    [SerializeField] Transform currentTurret;
    [SerializeField] IEnemyDamageHandler currentTurretDamage;
    [SerializeField] LayerMask objectsLayer = new LayerMask();

    protected Animator anim;
    protected State currentState;
    Material material;

    Vector3[] path;
    Vector3 currentWaypoint;
    Vector3 pushDirection;

    int targetIndex;

    float speed;
    float attackSpeed;
    float rotationSpeed;
    float stunDuration;
    float pushDistance;
    float fearDuration;

    bool isStunned;
    bool isFeared;
    bool isKnockbacked;
    bool isSlowed;
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
    public float AttackSpeed { get => attackSpeed; }

    public virtual void OnObjectSpawn()
    {
        setUpMaterial();
        title = info.name;
        currentHP = Info.MaxHealth;
        maxHP = Info.MaxHealth;
        anim = transform.Find("Model").GetComponent<Animator>();
        resetEnemyCC();
        speed = info.DefaultSpeed;
        rotationSpeed = info.InitRotationSpeed;
        attackSpeed = info.AttackSpeed;
        currentState = new Move(this, anim, goal);
        currentTurret = null;

        ActiveEnemies.Instance.enemiesList.Add(this);
    }

    public virtual void EnemyUpdate()
    {
        currentState = currentState.Process();
        //print(currentState.GetType()); //Debug states

        if (isTargetTurretDead()) currentTurret = null;
    }

    void OnDisable()
    {
        StopCoroutine("FollowPath");
    }

    public virtual void setNewGoal()
    {

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
        ActiveEnemies.Instance.enemiesList.Remove(this);
        Master.Instance.UpdateBalance(info.CoinsReward);
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

        if (currentTurret != null) newTarget = new PathData(currentTurret.position, currentTurret);
        else newTarget = new PathData(Goal.position, Goal);

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

    void setUpMaterial()
    {
        Transform model = transform.Find("Model");
        if (model != null)
        {
            Transform body = model.Find("Body");
            if (body != null)
            {
                Renderer renderer = body.GetComponent<Renderer>();
                material = renderer.material;
            }
        }
    }

    void changeMaterialColor(Color newColor)
    {
        if (material == null) return;
        material.color = newColor;
    }

    public void Stun(float secondsStunned)
    {
        if (!gameObject.activeSelf || isKnockbacked) return; //Can't stun an enemy that is knockbacked

        stunDuration = secondsStunned;
        isStunned = true;

        StopCoroutine("FollowPath");

        if(currentTurret != null) currentState = new Stun(this, anim, currentTurret);
        else currentState = new Stun(this, anim, goal);
    }

    float baseSpeed = 0.0f;
    float baseRotationSpeed = 0.0f;
    float baseAttackSpeed = 0.0f;
    Coroutine currentSlow = null;
    public void Slow(float secondsSlowed, float slowReduction)
    {
        if (!gameObject.activeSelf || isKnockbacked || isStunned || isFeared) return; //Can't slow an enemy that is knockbacked, stunned or feared

        if (currentSlow != null)
        {
            StopCoroutine(currentSlow);
            speed = baseSpeed;
            rotationSpeed = baseRotationSpeed;
            attackSpeed = baseAttackSpeed;
        }
        currentSlow = StartCoroutine(slowEnemy(secondsSlowed, slowReduction));
    }

    IEnumerator slowEnemy(float secondsSlowed, float slowReduction)
    {
        isSlowed = true;

        if (!isFeared)
        {
            baseSpeed = speed;
            baseRotationSpeed = rotationSpeed;
        }
        baseAttackSpeed = attackSpeed;

        speed *= slowReduction;
        rotationSpeed *= slowReduction;
        attackSpeed *= slowReduction;

        anim.SetFloat("animSpeed", slowReduction);

        yield return new WaitForSeconds(secondsSlowed);

        if (!isFeared)
        {
            speed = baseSpeed;
            rotationSpeed = baseRotationSpeed;

            anim.SetFloat("animSpeed", 1.0f);
        }
        attackSpeed = baseAttackSpeed;

        isSlowed = false;
    }

    Coroutine currentPoison = null;
    public void Poison(float secondsPoisoned, int damagePerSecond)
    {
        if (!gameObject.activeSelf) return;

        if (currentPoison != null)
        {
            StopCoroutine(currentPoison);
            changeMaterialColor(new Color(1, 1, 1));
        }
        currentPoison = StartCoroutine(poisonEnemy(secondsPoisoned, damagePerSecond));
    }

    IEnumerator poisonEnemy(float secondsPoisoned, float damagePerSecond)
    {
        changeMaterialColor(poisonedColor);

        int timer = 0;
        while (timer < secondsPoisoned)
        {
            yield return new WaitForSeconds(1f);
            GetDamage(Mathf.RoundToInt(damagePerSecond));
            timer++;
        }

        changeMaterialColor(Color.white);
        currentPoison = null;
    }

    public void Fear(float fearSeconds)
    {
        if (!gameObject.activeSelf || isKnockbacked || isStunned) return; //Can't fear an enemy that is knockbacked or stunned

        StartCoroutine(fearEnemy(fearSeconds)); //Fear also slows enemies

        fearDuration = fearSeconds;
        isFeared = true;

        StopCoroutine("FollowPath");

        if (currentTurret != null) currentState = new Fear(this, anim, currentTurret);
        else currentState = new Fear(this, anim, goal);
    }

    IEnumerator fearEnemy(float secondsFear)
    {
        if (!isSlowed)
        {
            baseSpeed = speed;
            baseRotationSpeed = rotationSpeed;
        }

        speed /= 2f;
        rotationSpeed *= 2f;

        anim.SetFloat("animSpeed", 0.5f);

        yield return new WaitForSeconds(secondsFear);

        if (!isSlowed)
        {
            speed = baseSpeed;
            rotationSpeed = baseRotationSpeed;
        }

        anim.SetFloat("animSpeed", 1f);
    }


    public void Knockback(float knockbackDistance, Vector3 knockbackDirection)
    {
        pushDistance = knockbackDistance;
        pushDirection = knockbackDirection;
        isKnockbacked = true;

        StopCoroutine("FollowPath");

        if (currentTurret != null) currentState = new Knockback(this, anim, currentTurret);
        else currentState = new Knockback(this, anim, goal);
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

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("TurretTrigger") && currentTurret == null && !isEnemyUnderCC())
        {
            currentTurretDamage = other.transform.parent.GetComponentInChildren<IEnemyDamageHandler>();
            currentTurret = other.transform.parent;
            currentState = new Move(this, anim, currentTurret);
        }
    }

    protected bool isEnemyUnderCC()
    {
        return isKnockbacked || isStunned || isFeared;
    }

    void resetEnemyCC()
    {
        isKnockbacked = false;
        isStunned = false;
        isFeared = false;
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