using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class EnemyDamageHandler : MonoBehaviour, IEnemyDamageHandler
{
    [SerializeField] List<EnemyDamage> enemyDamagesBehaviours = null;
    
    public void OnEnemyHit(float damage)
    {
        foreach (EnemyDamage enemyDamageBehaviour in enemyDamagesBehaviours) enemyDamageBehaviour.OnEnemyHit(damage);
    }

    public void AddEnemyDamageBehaviour(EnemyDamage enemyDamageBehaviour)
    {
        enemyDamagesBehaviours.Add(enemyDamageBehaviour);
    }
    public void RemoveEnemyDamageBehaviour(EnemyDamage enemyDamageBehaviour)
    {
        enemyDamagesBehaviours.Remove(enemyDamageBehaviour);
    }
}