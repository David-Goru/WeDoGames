using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnProjectilesAroundTurret : EffectComponent
{
    [SerializeField] Pool projectilePool = null;
    [SerializeField] float radius = 0.5f;
    [SerializeField] float height = 0.5f;
    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;
    ObjectPooler objectPooler;

    List<ITurretShotBehaviour> shotBehaviours = new List<ITurretShotBehaviour>();

    int projectilesSpawned = 0;
    bool isFull = false;
    float timer = 0f;

    event Action onProjectileDisabled;

    public override void InitializeComponent()
    {
        getDependencies();
        initializeMembers();
    }
    public override void UpdateComponent()
    {
        if (!isFull)
        {
            if (timer >= turretStats.GetStatValue(StatType.ATTACKSPEED))
            {
                spawn();
                timer = 0f;
            }
            else timer += Time.deltaTime;
        }
    }

    void getDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
        objectPooler = ObjectPooler.GetInstance();
        enemyDamageHandler = transform.parent.GetComponentInChildren<IEnemyDamageHandler>();
        shotBehaviours = GetComponents<ITurretShotBehaviour>().ToList();
    }

    void initializeMembers()
    {
        projectilesSpawned = 0;
        timer = 0f;
        isFull = false;
        if(onProjectileDisabled == null)
            onProjectileDisabled += decrementProjectilesSpawned;
    }

    void decrementProjectilesSpawned()
    {
        isFull = false;
        projectilesSpawned -= 1;
    }

    void spawn()
    {
        callShotBehaviours();
        createAndInitializeProjectile();
        checkIfIsFull();
    }

    void callShotBehaviours()
    {
        foreach (ITurretShotBehaviour shotBehaviour in shotBehaviours) shotBehaviour.OnShot();
    }

    void createAndInitializeProjectile()
    {
        GameObject obj = objectPooler.SpawnObject(projectilePool.tag, getRandomPositionAroundTurret(), Quaternion.identity);
        obj.GetComponent<SporeProjectile>().SetInfo(transform.parent, turretStats, enemyDamageHandler);
        obj.GetComponent<SporeProjectile>().SetSpawnerInfo(onProjectileDisabled);
        projectilesSpawned += 1;
    }

    Vector3 getRandomPositionAroundTurret()
    {
        int randAngle = UnityEngine.Random.Range(0, 360);
        Vector3 direction = Quaternion.Euler(0, randAngle, 0) * (Vector3.forward * radius + Vector3.up * height);
        Vector3 position = direction + transform.position;
        return position;
    }

    private void checkIfIsFull()
    {
        if (projectilesSpawned >= turretStats.GetStatValue(StatType.MAXSPORES))
        {
            isFull = true;
            timer = 0f;
        }
    }

    private void OnDisable()
    {
        onProjectileDisabled -= decrementProjectilesSpawned;
    }

}
