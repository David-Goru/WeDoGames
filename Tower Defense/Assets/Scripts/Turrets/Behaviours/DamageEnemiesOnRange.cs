using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class DamageEnemiesOnRange : MonoBehaviour, ITurretBehaviour
{
    ICurrentTargetsOnRange targetDetection;
    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;
    [SerializeField] ParticleSystem particles = null;

    float timer = 0f;

    public void InitializeBehaviour()
    {
        GetDependencies();
        timer = 0f;
    }

    public void UpdateBehaviour()
    {
        if(timer >= turretStats.GetStatValue(StatType.ATTACKRATE))
        {
            if (ReferenceEquals(targetDetection, null)) return;
            float damage = turretStats.GetStatValue(StatType.DAMAGE);
            particles.Play();
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
        else
        {
            timer += Time.deltaTime;
        }
    }

    void GetDependencies()
    {
        targetDetection = GetComponent<ICurrentTargetsOnRange>();
        turretStats = GetComponent<TurretStats>();
        enemyDamageHandler = GetComponent<IEnemyDamageHandler>();
    }
}