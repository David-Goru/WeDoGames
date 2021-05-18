using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class handles turret damage taken
/// </summary>
public class TurretDamageable : EnemyDamage
{
    TurretStats turretStats;
    [SerializeField] List<TurretNoHealth> turretNoHealthBehaviours = null;

    private void Awake()
    {
        turretStats = GetComponentInParent<TurretStats>();
    }

    public override void OnEnemyHit(float damage)
    {
        turretStats.currentHp -= damage;
        if (turretStats.currentHp <= 0)
        {
            if (turretNoHealthBehaviours.Count == 0)
            {
                Debug.LogError("Your turret " + gameObject.name +" doesn't have any Behaviour when it has no health ");
                return;
            }
            foreach (var noHealthBehaviour in turretNoHealthBehaviours)
            {
                noHealthBehaviour.OnTurretNoHealth();
            }
        }
    }
}