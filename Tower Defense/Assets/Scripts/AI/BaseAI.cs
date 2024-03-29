﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class BaseAI : Entity, ITurretDamage, IPooledObject, IStunnable, ISlowable, IFearable, IDamageable, IPoisonable, IKnockbackable, IDamageReductible
{
    [SerializeField] EnemyInfo info = null;
    [SerializeField] Color poisonedColor = Color.magenta;
    [SerializeField] LayerMask objectsLayer = new LayerMask();

    [Header("Particles Positions")]
    [SerializeField] Transform particlesSpawnPos;
    [SerializeField] Transform hitParticlesSpawnPos;
    [SerializeField] Transform deathParticlesSpawnPos;

    [Header("Attack sounds")]
    [SerializeField] List<AudioClip> attackSounds = new List<AudioClip>();
    List<AudioClip> usedAttackSounds = new List<AudioClip>();

    [Header("Debug")]
    [SerializeField] protected Transform goal;
    [SerializeField] Transform currentTurret;
    [SerializeField] IEnemyDamageHandler currentTurretDamage;

    protected Animator anim;
    protected State currentState;
    Material material;
    AudioSource audioSource;

    Vector3[] path;
    Vector3 currentWaypoint;
    Vector3 pushDirection;

    Coroutine currentSlow = null;
    Coroutine currentDamageReduction = null;
    Coroutine currentPoison = null;
    Coroutine currentDeath = null;

    GameObject poisonVFX = null;
    GameObject slowVFX = null;
    GameObject damageVFX = null;
    GameObject hitVFX = null;
    GameObject coinVFX = null;
    GameObject deathVFX = null;

    ObjectPooler objectPool;

    int targetIndex;
    int damage;

    float speed;
    float attackSpeed;
    float rotationSpeed;
    float stunDuration;
    float pushDistance;
    float fearDuration;

    bool isStunned;
    bool isFeared;
    bool isKnockbacked;
    bool isDying;
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
    public bool IsDying { get => isDying; }
    public bool IsStunned { get => isStunned; set => isStunned = value; }
    public bool IsKnockbacked { get => isKnockbacked; set => isKnockbacked = value; }
    public float FearDuration { get => fearDuration; set => fearDuration = value; }
    public bool IsFeared { get => isFeared; set => isFeared = value; }
    public float AttackSpeed { get => attackSpeed; }
    public float Speed { get => speed; set => speed = value; }
    public int Damage { get => damage; }
    public Transform ParticlesSpawnPos { get => particlesSpawnPos; }
    public ObjectPooler ObjectPool { get => objectPool; }

    void Awake()
    {
        setUpMaterial();
        anim = transform.Find("Model").GetComponent<Animator>();
        objectPool = ObjectPooler.GetInstance();
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void OnObjectSpawn()
    {
        title = info.name;
        currentHP = Info.MaxHealth;
        maxHP = Info.MaxHealth;
        resetEnemyCC();
        returnToPoolVFX();
        overrideCoroutines();
        changeMaterialColor(Color.white);
        speed = info.DefaultSpeed;
        rotationSpeed = info.InitRotationSpeed;
        attackSpeed = info.AttackSpeed;
        damage = info.Damage;
        currentState = new Move(this, anim, goal);
        currentTurret = null;
        slowVFX = null;
        poisonVFX = null;
        damageVFX = null;
        hitVFX = null;
        currentDeath = null;
        transform.LookAt(goal);
        anim.SetFloat("animSpeed", 1f);

        if(deathVFX != null) objectPool.ReturnToThePool(deathVFX.transform);

        ActiveEnemies.Instance.enemiesList.Add(this);
    }

    public virtual void EnemyUpdate()
    {
        if(!isDying) currentState = currentState.Process();
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
        returnToPoolVFX();

        if(currentDeath == null)
        {
            currentState.Exit();

            isDying = true;

            anim.SetTrigger("DIE");
            coinVFX = objectPool.SpawnObject("CoinVFX", particlesSpawnPos.position);
            Master.Instance.UpdateBalance(info.CoinsReward);

            StopAllCoroutines();

            currentDeath = StartCoroutine(enemyDeath());
        }
    }

    IEnumerator enemyDeath()
    {
        yield return new WaitForSeconds(1f);

        objectPool.ReturnToThePool(coinVFX.transform);
        deathVFX = objectPool.SpawnObject("DeathVFX", deathParticlesSpawnPos.position);

        ObjectPooler.GetInstance().ReturnToThePool(this.transform);
        Waves.KillEnemy();
        ActiveEnemies.Instance.enemiesList.Remove(this);

        anim.ResetTrigger("DIE");
        isDying = false;
    }

    void overrideCoroutines()
    {
        currentSlow = null;
        currentDamageReduction = null;
        currentPoison = null;
        currentDeath = null;
    }

    void returnToPoolVFX()
    {
        if (poisonVFX != null) objectPool.ReturnToThePool(poisonVFX.transform);
        if (slowVFX != null) objectPool.ReturnToThePool(slowVFX.transform);
        if (damageVFX != null) objectPool.ReturnToThePool(damageVFX.transform);
        if (hitVFX != null) objectPool.ReturnToThePool(hitVFX.transform);
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

    public void OnTurretHit(Transform turretTransform, int _damage, IEnemyDamageHandler enemyDamage)
    {
        currentHP -= _damage;

        if (hitVFX == null) spawnHitVFX();
        else
        {
            objectPool.ReturnToThePool(hitVFX.transform);
            spawnHitVFX();
        }

        if (checkDeath()) return;
        if (currentTurret == null || currentState.Target == Nexus.Instance.transform)
        {
            currentTurretDamage = enemyDamage;
            currentState.OnTurretHit(turretTransform);
        }
    }

    void spawnHitVFX()
    {
        hitVFX = objectPool.SpawnObject("HitVFX", hitParticlesSpawnPos.position);
        hitVFX.transform.SetParent(hitParticlesSpawnPos);
        hitVFX.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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
        while (!isStunned)
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
        if (!gameObject.activeSelf || isKnockbacked || isDying) return; //Can't stun an enemy that is knockbacked

        StopCoroutine("FollowPath");

        currentState.Exit();

        stunDuration = secondsStunned;

        if (currentTurret != null) currentState = new Stun(this, anim, currentTurret);
        else currentState = new Stun(this, anim, goal);
    }

    public void Slow(float secondsSlowed, float slowReduction)
    {
        if (!gameObject.activeSelf || isDying) return;

        if (currentSlow != null)
        {
            StopCoroutine(currentSlow);
            resetSlowValues();
            if(slowVFX != null) objectPool.ReturnToThePool(slowVFX.transform);
            slowVFX = null;
        }
        currentSlow = StartCoroutine(slowEnemy(secondsSlowed, slowReduction));
    }

    IEnumerator slowEnemy(float secondsSlowed, float slowReduction)
    {
        if (slowVFX == null)
        {
            slowVFX = objectPool.SpawnObject("SlowVFX", particlesSpawnPos.position);
            slowVFX.transform.SetParent(particlesSpawnPos);
            slowVFX.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        speed *= slowReduction;
        rotationSpeed *= slowReduction;
        attackSpeed *= slowReduction;

        anim.SetFloat("animSpeed", 0.5f);

        yield return new WaitForSeconds(secondsSlowed);

        resetSlowValues();

        objectPool.ReturnToThePool(slowVFX.transform);
        slowVFX = null;
        currentSlow = null;
    }

    void resetSlowValues()
    {
        speed = info.DefaultSpeed;
        rotationSpeed = info.InitRotationSpeed;
        attackSpeed = info.AttackSpeed;

        anim.SetFloat("animSpeed", 1f);
    }

    public void Poison(float secondsPoisoned, int damagePerSecond)
    {
        if (!gameObject.activeSelf || isDying) return;

        if (currentPoison != null)
        {
            StopCoroutine(currentPoison);
            changeMaterialColor(new Color(1, 1, 1));
            if (poisonVFX != null) objectPool.ReturnToThePool(poisonVFX.transform);
            poisonVFX = null;
        }
        currentPoison = StartCoroutine(poisonEnemy(secondsPoisoned, damagePerSecond));
    }

    IEnumerator poisonEnemy(float secondsPoisoned, float damagePerSecond)
    {
        changeMaterialColor(poisonedColor);
        if (poisonVFX == null)
        {
            poisonVFX = objectPool.SpawnObject("PoisonVFX", particlesSpawnPos.position);
            poisonVFX.transform.SetParent(particlesSpawnPos);
            poisonVFX.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        int timer = 0;
        while (timer < secondsPoisoned)
        {
            yield return new WaitForSeconds(1f);
            recieveDamage(Mathf.RoundToInt(damagePerSecond));
            timer++;
        }

        objectPool.ReturnToThePool(poisonVFX.transform);
        poisonVFX = null;
        changeMaterialColor(Color.white);
        currentPoison = null;
    }

    public void Fear(float fearSeconds)
    {
        if (!gameObject.activeSelf || isKnockbacked || isStunned || isDying) return; //Can't fear an enemy that is knockbacked or stunned

        if (currentSlow != null) //Override slow
        {
            StopCoroutine(currentSlow);
            resetSlowValues();
            if (slowVFX != null) objectPool.ReturnToThePool(slowVFX.transform);
            slowVFX = null;
            currentSlow = null;
        }

        StopCoroutine("FollowPath");

        currentState.Exit();

        fearDuration = fearSeconds;

        if (currentTurret != null) currentState = new Fear(this, anim, currentTurret);
        else currentState = new Fear(this, anim, goal);
    }


    public void Knockback(float knockbackDistance, Vector3 knockbackDirection)
    {
        if (!gameObject.activeSelf || isDying) return;

        StopCoroutine("FollowPath");

        currentState.Exit();

        pushDistance = knockbackDistance;
        pushDirection = knockbackDirection;

        if (currentTurret != null) currentState = new Knockback(this, anim, currentTurret);
        else currentState = new Knockback(this, anim, goal);
    }

    public void ReduceDamage(float secondsDamageReduced, float damageReduction)
    {
        if (!gameObject.activeSelf || isDying) return;

        if(currentDamageReduction != null)
        {
            StopCoroutine(currentDamageReduction);
            damage = info.Damage;
        }
        currentDamageReduction = StartCoroutine(changeDamage(secondsDamageReduced, damageReduction));
    }

    IEnumerator changeDamage(float seconds, float damageReduction)
    {
        float newDamage = damage * damageReduction;

        damage = (int)Mathf.Floor(newDamage);

        yield return new WaitForSeconds(seconds);

        damage = info.Damage;

        currentDamageReduction = null;
    }

    public void GetDamage(int _damage)
    {
        if (damageVFX == null) spawnDamageVFX();
        else
        {
            objectPool.ReturnToThePool(damageVFX.transform);
            spawnDamageVFX();
        }

        recieveDamage(_damage);
    }

    void recieveDamage(int _damage)
    {
        currentHP -= _damage;

        checkDeath();
    }

    void spawnDamageVFX()
    {
        damageVFX = objectPool.SpawnObject("DamageVFX", particlesSpawnPos.position);
        damageVFX.transform.SetParent(particlesSpawnPos);
        damageVFX.transform.localScale = new Vector3(1f, 1f, 1f);
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

    public void PlayRandomAttack()
    {
        AudioClip attackClip = attackSounds[Random.Range(0, attackSounds.Count)];

        usedAttackSounds.Add(attackClip);
        attackSounds.Remove(attackClip);

        audioSource.clip = attackClip;
        audioSource.Play();

        if(attackSounds.Count == 0)
        {
            attackSounds.AddRange(usedAttackSounds);
            usedAttackSounds.Clear();
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