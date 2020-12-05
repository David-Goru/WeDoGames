using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class handles turret damage taken
/// </summary>
public class TurretDamageable : MonoBehaviour, IEnemyDamage
{
    TurretStats turretStats;
    List<ITurretNoHealth> turretNoHealthBehaviours = new List<ITurretNoHealth>();

    private void Awake()
    {
        turretStats = GetComponent<TurretStats>();
        turretNoHealthBehaviours = GetComponents<ITurretNoHealth>().ToList();
    }

    public void OnEnemyHit(float damage)
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