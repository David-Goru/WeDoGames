using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret", menuName = "ScriptableObjects/TurretInfo", order = 0)]
public class TurretInfo : BuildingInfo
{
    [SerializeField] float attackRate = 0;
    [SerializeField] float attackDamage = 0;
    [SerializeField] float attackRange = 0;

    public float GetAttackRate() { return attackRate; }
    public float GetAttackDamage() { return attackDamage; }
    public float GetAttackRange() { return attackRange; }

}
