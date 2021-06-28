using System;
using UnityEngine;

public class ConstantDamageToEnemiesInRange : EffectComponent
{
    [SerializeField] CurrentTargetsOnRange targetDetection = null;
    [SerializeField] ParticleSystem particles = null;
    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;

    public override void InitializeComponent()
    {
        GetDependencies();
    }

    public override void UpdateComponent()
    {
        doDamage();
    }

    void GetDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
        enemyDamageHandler = transform.parent.GetComponentInChildren<IEnemyDamageHandler>();
    }

    void doDamage()
    {
        if (ReferenceEquals(targetDetection, null)) return;
        playParticles();

        float damage = turretStats.GetStatValue(StatType.DAMAGE);
        foreach (Transform target in targetDetection.CurrentTargets)
        {
            ITurretDamage turretDamageable = target.GetComponent<ITurretDamage>();
            if (turretDamageable != null) turretDamageable.OnTurretHit(transform, damage * Time.deltaTime, enemyDamageHandler);
        }
    }

    void playParticles()
    {
        if (particles != null) particles.Play();
    }
}