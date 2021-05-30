using UnityEngine;

public class ConstantDamageToEnemiesInRange : EffectComponent
{
    [SerializeField] CurrentTargetsOnRange targetDetection;
    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;
    [SerializeField] ParticleSystem particles = null;

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
        if (particles != null) particles.Play();

        float damage = turretStats.GetStatValue(StatType.DAMAGE);
        foreach (Transform target in targetDetection.CurrentTargets)
        {
            ITurretDamage turretDamageable = target.GetComponent<ITurretDamage>();
            if (turretDamageable != null)
            {
                turretDamageable.OnTurretHit(transform, damage * Time.deltaTime, enemyDamageHandler);
            }
        }
    }
}