using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class handles turret damage taken
/// </summary>
public class TurretDamageable : EnemyDamage
{
    [SerializeField] List<TurretNoHealth> turretNoHealthBehaviours = null;
    TurretStats turretStats;

    private void Awake()
    {
        turretStats = GetComponentInParent<TurretStats>();
    }

    public override void OnEnemyHit(int damage)
    {
        turretStats.CurrentHP -= damage;
        if (turretStats.CurrentHP <= 0)
        {
            Master.Instance.ActiveTurrets.Remove(transform.parent.gameObject);
            turretStats.BuildingInfo.NumberOfTurretsPlaced--;

            if (turretNoHealthBehaviours.Count == 0)
            {
                Debug.LogError("Your turret " + gameObject.name +" doesn't have any Behaviour when it has no health ");
                return;
            }
            foreach (var noHealthBehaviour in turretNoHealthBehaviours) noHealthBehaviour.OnTurretNoHealth();
        }
    }
}