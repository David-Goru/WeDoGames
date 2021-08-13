﻿using System;
using UnityEngine;

public class ShootToEnemy : EffectComponent
{
    [SerializeField] Pool projectile = null;
    [SerializeField] Transform spawnPosition = null;
    [SerializeField] Animator anim = null;
    [SerializeField] CurrentTargetsOnRange targetDetection = null;
    [SerializeField] AudioSource audioSource = null;
    [SerializeField] ParticleSystem particles = null;

    ObjectPooler objectPooler;
    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;
    float timer = 0;

    public CurrentTargetsOnRange TargetDetection { set { targetDetection = value; } get { return targetDetection; } }

    public override void InitializeComponent()
    {
        GetDependencies();
        resetTimer();
    }

    public override void UpdateComponent()
    {
        shootEnemies();
    }

    void GetDependencies()
    {
        objectPooler = ObjectPooler.GetInstance();
        turretStats = GetComponentInParent<TurretStats>();
        targetDetection = GetComponent<CurrentTargetsOnRange>();
        enemyDamageHandler = transform.parent.GetComponentInChildren<IEnemyDamageHandler>();
    }

    void shootEnemies()
    {
        if (ReferenceEquals(targetDetection, null)) return;
        foreach (Transform target in targetDetection.CurrentTargets) shootEnemy(target);
    }

    void shootEnemy(Transform enemy)
    {
        if(enemy == null)
        {
            resetTimer();
            return;
        }
        if(timer >= turretStats.GetStatValue(StatType.ATTACKSPEED))
        {
            spawnAndInitializeProjectile(enemy);
            doAnimation();
            playSound();
            playParticles();
            resetTimer();
        }
        else timer += Time.deltaTime;
    }

    void playParticles()
    {
        if(particles != null) particles.Play();
    }

    void spawnAndInitializeProjectile(Transform enemy)
    {
        GameObject obj = objectPooler.SpawnObject(projectile.tag, spawnPosition.position);
        obj.GetComponent<Projectile>().SetInfo(enemy, transform.parent, turretStats, enemyDamageHandler);
    }

    void doAnimation()
    {
        if (anim != null) anim.SetTrigger("Shoot");
    }

    void playSound()
    {
        if (audioSource != null) audioSource.Play();
    }

    void resetTimer()
    {
        timer = 0;
    }
}