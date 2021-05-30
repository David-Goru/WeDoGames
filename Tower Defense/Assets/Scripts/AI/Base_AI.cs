using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// <summary>
// Base class for AI enemies. It will trigger the initial behaviours and make the calls to the pathfinding system.
// </summary>

[SelectionBase]
public class Base_AI : Entity, ITurretDamage, IPooledObject, IStunnable, ISlowable, IFearable, IDamageable, IPoisonable
{
    [SerializeField] float damage = 0f;
    [SerializeField] float attackSpeed = 0f;
    [SerializeField] float range = 0f;
    [SerializeField] float defaultSpeed = 5f;
    [SerializeField] float initRotationSpeed = 300f;

    [Header("Debug")]
    [SerializeField] Transform goal;
    [SerializeField] Transform currentTurret;
    [SerializeField] IEnemyDamageHandler currentTurretDamage;
    [SerializeField] LayerMask objectsLayer;

    Animator anim;
    State currentState;
    float speed;
    float rotationSpeed;
    Vector3[] path;
    int targetIndex;
    bool pathReached;
    bool pathSuccessful;
    Vector3 currentWaypoint;
    float stunDuration;
    bool isStunned;
    float fearDuration;
    bool isFeared;

    public float Damage { get => damage; set => damage = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float Range { get => range; set => range = value; }
    public Transform Goal { get => goal; set => goal = value; }
    public Transform CurrentTurret { get => currentTurret; set => currentTurret = value; }
    public IEnemyDamageHandler CurrentTurretDamage { get => currentTurretDamage; set => currentTurretDamage = value; }
    public bool PathReached { get => pathReached; set => pathReached = value; }
    public bool PathSuccessful { get => pathSuccessful; set => pathSuccessful = value; }
    public float StunDuration { get => stunDuration; set => stunDuration = value; }
    public bool IsStunned { get => isStunned; set => isStunned = value; }
    public float FearDuration { get => fearDuration; set => fearDuration = value; }
    public bool IsFeared { get => isFeared; set => isFeared = value; }

    public void OnObjectSpawn()
    {
        goal = Nexus.GetTransform;
        currentHP = maxHP;
        anim = transform.Find("Model").GetComponent<Animator>();
        isFeared = false;
        isStunned = false;
        speed = defaultSpeed;
        rotationSpeed = initRotationSpeed;
        currentState = new Move(this, anim, goal);
        currentTurret = null;

        EnemiesActive.Instance.enemiesList.Add(this);
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game") return;
        goal = Nexus.GetTransform;
        currentHP = maxHP;
        anim = transform.Find("Model").GetComponent<Animator>();
        currentState = new Move(this, anim, goal);
        currentTurret = null;
    }

    public void EnemyUpdate()
    {
        currentState = currentState.Process();

        if (currentTurret != null && !currentTurret.gameObject.activeSelf) currentTurret = null;
    }

    void OnDisable()
    {
        StopCoroutine("FollowPath");
    }

    bool checkDeath()
    {
        if (currentHP <= 0 && gameObject.activeSelf)
        {
            StopAllCoroutines();
            ObjectPooler.GetInstance().ReturnToThePool(this.transform);
            WavesHandler.EnemyKilled();
            EnemiesActive.Instance.enemiesList.Remove(this);

            return true;
        }
        return false;
    }

    public void checkPath() //Called if a new object is spawned. Checks if the path should be recalculated (i.e. a new turret is in your way)
    {
        if (!pathReached)
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

        PathRequestManager.RequestPath(transform.position, newTarget, Range, OnPathFound);
    }

    public void OnTurretHit(Transform turretTransform, float damage, IEnemyDamageHandler enemyDamage)
    {
        currentHP -= Mathf.RoundToInt(damage);

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

        currentState = new Stun(this, anim, goal);
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

        speed -= speed * slowReduction;
        rotationSpeed -= rotationSpeed * slowReduction;

        anim.SetFloat("animSpeed", slowReduction);

        yield return new WaitForSeconds(secondsSlowed);

        speed = baseSpeed;
        rotationSpeed = baseRotationSpeed;

        anim.SetFloat("animSpeed", 1.0f);
    }

    Coroutine currentPoison = null;
    public void Poison(float secondsPoisoned, float damagePerSecond)
    {
        if (!gameObject.activeSelf) return;

        if (currentPoison != null) StopCoroutine(currentPoison);
        currentPoison = StartCoroutine(poisonEnemy(secondsPoisoned, damagePerSecond));
    }

    IEnumerator poisonEnemy(float secondsPoisoned, float damagePerSecond)
    {
        // Enable visual effects?

        int timer = 0;
        while (timer < secondsPoisoned)
        {
            yield return new WaitForSeconds(1.0f);
            GetDamage(damagePerSecond);
            timer++;
        }

        // Disable visual effects?

        currentPoison = null;
    }

    public void Fear(float fearSeconds)
    {
        StartCoroutine(fearEnemy(fearSeconds)); //Fear also slows enemies

        fearDuration = fearSeconds;
        isFeared = true;

        StopCoroutine("FollowPath");

        currentState = new Fear(this, anim, goal);
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

    public void GetDamage(float damage)
    {
        currentHP -= Mathf.RoundToInt(damage);

        checkDeath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Turret") && currentTurret == null)
        {
            currentTurret = other.transform.parent;
            currentState = new Move(this, anim, currentTurret);
        }
    }

    /* UI */
    const string info = @"Attack Range: {0:0.##}
Attack Rate: {1:0.##}
Attack Damage: {2:0.##}
Speed: {3:0.##}
{4}";

    public override string GetExtraInfo()
    {
        string states = "";
        if (currentSlow != null) states += "slowed, ";
        if (currentPoison != null) states += "poisoned, ";
        if (isFeared) states += "feared, ";
        if (isStunned) states += "stunned, ";

        if (states.Length > 2)
        {
            states = states.Remove(states.Length - 2);
            states = char.ToUpper(states[0]) + states.Substring(1);
        }

        return string.Format(info, range, attackSpeed, damage, speed, states);
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