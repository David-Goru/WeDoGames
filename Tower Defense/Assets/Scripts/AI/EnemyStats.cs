using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyStats", order = 0)]
public class EnemyStats : BuildingInfo
{

    [SerializeField] float attackRate;
    [SerializeField] float attackDamage;
    [SerializeField] float attackRange;

    public float GetAttackRate() { return attackRate; }
    public float GetAttackDamage() { return attackDamage; }
    public float GetAttackRange() { return attackRange; }
}
