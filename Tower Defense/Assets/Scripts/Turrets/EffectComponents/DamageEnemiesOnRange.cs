using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class DamageEnemiesOnRange : EffectComponent
{
    [SerializeField] CurrentTargetsOnRange targetDetection = null;
    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;
    [SerializeField] ParticleSystem particles = null;

    float timer = 0f;

    public override void InitializeComponent()
    {
        GetDependencies();
        timer = 0f;
    }

    public override void UpdateComponent()
    {
        if (timer >= turretStats.GetStatValue(StatType.ATTACKSPEED))
        {
            doDamage();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    void GetDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
        enemyDamageHandler = transform.parent.GetComponentInChildren<IEnemyDamageHandler>();
    }

    void doDamage()
    {
        if (ReferenceEquals(targetDetection, null)) return;
        if(particles != null) particles.Play();

        float damage = turretStats.GetStatValue(StatType.DAMAGE);
        foreach (Transform target in targetDetection.CurrentTargets)
        {
            ITurretDamage turretDamageable = target.GetComponent<ITurretDamage>();
            if (turretDamageable != null)
            {
                turretDamageable.OnTurretHit(transform, damage, enemyDamageHandler);
            }
        }
        timer = 0f;
    }
}
