using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret", menuName = "ScriptableObjects/TurretInfo", order = 0)]
public class TurretInfo : ScriptableObject
{
    [SerializeField] float hp;
    [SerializeField] float attackRate;
    [SerializeField] float attackDamage;
    [SerializeField] float attackRange;

    public float GetHp() { return hp; }
    public float GetAttackRate() { return attackRate; }
    public float GetAttackDamage() { return attackDamage; }
    public float GetAttackRange() { return attackRange; }
}
