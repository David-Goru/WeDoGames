using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class handles turret damage taken
/// </summary>
public class TurretDamageable : EnemyDamage
{
    List<TurretNoHealth> turretNoHealthBehaviours;
    TurretStats turretStats;
    HealthSlider healthSlider;

    void Awake()
    {
        turretStats = GetComponentInParent<TurretStats>();
        turretNoHealthBehaviours = GetComponents<TurretNoHealth>().ToList();
        healthSlider = transform.parent.GetComponentInChildren<HealthSlider>();
    }

    public override void OnEnemyHit(int damage)
    {
        turretStats.CurrentHP -= damage;
        float maxHp = turretStats.MaxHP;
        healthSlider.SetFillAmount(turretStats.CurrentHP / maxHp);

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