using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectilesAroundTurret : EffectComponent
{
    [SerializeField] ParticleSystem particles = null;
    [SerializeField] Pool projectilePool = null;
    [SerializeField] Animator anim = null;
    TurretStats turretStats;
    ObjectPooler objectPooler;

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
    }

    private void initializeMembers()
    {
        projectilesSpawned = 0;
        timer = 0f;
        isFull = false;
        onProjectileDisabled += decrementProjectilesSpawned;
    }

    void decrementProjectilesSpawned()
    {
        isFull = false;
        projectilesSpawned -= 1;
    }

    void spawn()
    {
        playAnimation();
        playParticles();
        createAndInitializeProjectile();
        checkIfIsFull();
    }

    void playAnimation()
    {
        if (anim != null) anim.SetTrigger("Shoot");
    }

    void playParticles()
    {
        if (particles != null) particles.Play();
    }

    void createAndInitializeProjectile()
    {
        GameObject obj = objectPooler.SpawnObject(projectilePool.tag, getRandomPositionAroundTurret(), Quaternion.identity);
        obj.GetComponent<SporeProjectile>().SetInfo(transform.parent, turretStats);
        obj.GetComponent<SporeProjectile>().SetSpawnerInfo(onProjectileDisabled);
        projectilesSpawned += 1;
    }

    Vector3 getRandomPositionAroundTurret()
    {
        float radius = 0.5f;
        float height = 0.5f;
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
