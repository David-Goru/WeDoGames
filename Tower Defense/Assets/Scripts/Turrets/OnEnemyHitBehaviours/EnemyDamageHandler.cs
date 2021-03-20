using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class EnemyDamageHandler : MonoBehaviour, IEnemyDamageHandler
{
    List<IEnemyDamage> enemyDamagesBehaviours;
    
    private void Awake()
    {
        enemyDamagesBehaviours = transform.GetComponents<IEnemyDamage>().ToList();
    }

    public void OnEnemyHit(float damage)
    {
        foreach (IEnemyDamage enemyDamageBehaviour in enemyDamagesBehaviours)
        {
            enemyDamageBehaviour.OnEnemyHit(damage);
        }
    }

    public void AddEnemyDamageBehaviour(IEnemyDamage enemyDamageBehaviour)
    {
        enemyDamagesBehaviours.Add(enemyDamageBehaviour);
    }
    public void RemoveEnemyDamageBehaviour(IEnemyDamage enemyDamageBehaviour)
    {
        enemyDamagesBehaviours.Remove(enemyDamageBehaviour);
    }
}