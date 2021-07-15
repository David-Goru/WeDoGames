using UnityEngine;

public class ConstantDamageToEnemiesInRange : EffectComponent
{
    [SerializeField] CurrentTargetsOnRange targetDetection = null;
    [SerializeField] ParticleSystem particles = null;
    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;

    float timer = 0f;
    float damageInterval = 0f;
    float damage = 0f;

    public override void InitializeComponent()
    {
        getDependencies();
        initMembers();
    }

    public override void UpdateComponent()
    {
        checkIfDamageChanged();
        if (timer >= damageInterval)
        {
            doDamage();
            timer = 0f;
        }
        else timer += Time.deltaTime;
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
