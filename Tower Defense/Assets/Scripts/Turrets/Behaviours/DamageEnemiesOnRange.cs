using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class DamageEnemiesOnRange : MonoBehaviour, ITurretBehaviour
{
    ICurrentTargetsOnRange targetDetection;
    BuildingRange turretBuildingRange;
    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;
    [SerializeField] ParticleSystem particles;

    float timer = 0f;

    public void InitializeBehaviour()
    {
        GetDependencies();
        timer = 0f;
    }

    public void UpdateBehaviour()
    {
        if(timer >= turretStats.AttackRate)
        {
            if (ReferenceEquals(targetDetection, null)) return;
            float damage = turretStats.AttackDamage;
            particles.Play();
            foreach (Transform target in targetDetection.CurrentTargets)
            {
                ITurretDamage turretDamageable = target.GetComponent<ITurretDamage>();
                if (turretDamageable != null)
                {
                    turretDamageable.OnTurretHit(turretBuildingRange, damage, enemyDamageHandler);
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
        turretBuildingRange = GetComponent<BuildingRange>();
        turretStats = GetComponent<TurretStats>();
        enemyDamageHandler = GetComponent<IEnemyDamageHandler>();
    }
}