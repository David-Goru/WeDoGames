using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootToEnemy : MonoBehaviour, ITurretBehaviour
{
    [SerializeField] Pool projectile = null;
    [SerializeField] Transform spawnPosition = null;
    ObjectPooler objectPooler;
    GameObject obj;

    float timer = 0;

    TurretStats turretStats;
    ICurrentTargetsOnRange enemyDetection;
    public ICurrentTargetsOnRange EnemyDetection { set { enemyDetection = value; } get { return enemyDetection; } }

    public void ShootEnemy(Transform enemy, float damage, Turret turret)
    {
        if(enemyDetection.CurrentTargets[0] == null)
        {
            ResetTimer();
            return;
        }
        if(timer >= turretStats.AttackRate)
        {
            obj = objectPooler.SpawnObject(projectile.tag, spawnPosition.position);
            obj.GetComponent<Projectile>().SetInfo(enemy, damage, turret);
            ResetTimer();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    public void InitializeBehaviour()
    {
        objectPooler = ObjectPooler.GetInstance();
        turretStats = transform.root.GetComponentInChildren<TurretStats>();
        enemyDetection = transform.root.GetComponentInChildren<ICurrentTargetsOnRange>();
    }

    public void UpdateBehaviour()
    {
        ShootEnemy(enemyDetection.CurrentTargets[0], turretStats.AttackDamage, null);
    }
}
