using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class DamageEnemiesOnRange : EffectComponent
{
    [SerializeField] CurrentTargetsOnRange targetDetection = null;
    [SerializeField] ParticleSystem particles = null;
    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;

    float timer = 0f;

    public override void InitializeComponent()
    {
        GetDependencies();
        timer = 0f;
    }

    public override void UpdateComponent()
    {
        if (isTimeToDamage()) doDamage();
        else timer += Time.deltaTime;
    }

    void GetDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
        enemyDamageHandler = transform.parent.GetComponentInChildren<IEnemyDamageHandler>();
    }

    bool isTimeToDamage()
    {
        return timer >= turretStats.GetStatValue(StatType.ATTACKSPEED);
    }

    void doDamage()
    {
        if (ReferenceEquals(targetDetection, null)) return;
        playParticles();

        int damage = (int)turretStats.GetStatValue(StatType.DAMAGE);
        foreach (Transform target in targetDetection.CurrentTargets)
        {
            ITurretDamage turretDamageable = target.GetComponent<ITurretDamage>();
            if (turretDamageable != null) turretDamageable.OnTurretHit(transform, damage, enemyDamageHandler);
        }
        timer = 0f;
    }
    
    void playParticles()
    {
        if (particles != null) particles.Play();
    }
}
