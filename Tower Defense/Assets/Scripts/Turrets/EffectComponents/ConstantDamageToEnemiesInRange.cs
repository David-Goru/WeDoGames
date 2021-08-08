using System;
using UnityEngine;

public class ConstantDamageToEnemiesInRange : EffectComponent
{
    [SerializeField] CurrentTargetsOnRange targetDetection = null;
    [SerializeField] ParticleSystem particles = null;
    [SerializeField] Animator anim = null;
    [SerializeField] AudioSource audioSource = null;

    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;

    float timer = 0f;
    float damageInterval = 0f;
    float damage = 0f;

    bool areTargetsInRange = false;

    public override void InitializeComponent()
    {
        getDependencies();
        initMembers();
    }

    public override void UpdateComponent()
    {
        areTargetsInRange = targetDetection.AreTargetsInRange;
        setAnimationState();
        setSound();
        checkIfDamageChanged();
        if (timer >= damageInterval)
        {
            doDamage();
            timer = 0f;
        }
        else timer += Time.deltaTime;
    }

    void setAnimationState()
    {
        if (anim != null) anim.SetBool("IsShooting", targetDetection.AreTargetsInRange);
    }

    void setSound()
    {
        if (!audioSource.isPlaying && areTargetsInRange) audioSource.Play();
        else if (audioSource.isPlaying && !areTargetsInRange) audioSource.Stop();
    }

    void getDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
        enemyDamageHandler = transform.parent.GetComponentInChildren<IEnemyDamageHandler>();
    }

    void initMembers()
    {
        timer = 0f;
        damage = turretStats.GetStatValue(StatType.DAMAGE);
        damageInterval = 1 / damage;
        areTargetsInRange = false;
    }

    void checkIfDamageChanged()
    {
        float currentDamage = turretStats.GetStatValue(StatType.DAMAGE);
        if (damage != currentDamage)
        {
            damage = currentDamage;
            damageInterval = 1 / damage;
        }
    }

    void doDamage()
    {
        if (ReferenceEquals(targetDetection, null)) return;
        playParticles();

        foreach (Transform target in targetDetection.CurrentTargets)
        {
            ITurretDamage turretDamageable = target.GetComponent<ITurretDamage>();
            if (turretDamageable != null) turretDamageable.OnTurretHit(transform, 1, enemyDamageHandler);
        }
    }

    void playParticles()
    {
        if (particles != null) particles.Play();
    }
}
