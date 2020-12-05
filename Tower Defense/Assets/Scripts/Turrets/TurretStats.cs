using UnityEngine;

/// <summary>
/// This class stores all the stats of the turret
/// </summary>
public class TurretStats : MonoBehaviour
{
    [SerializeField] TurretInfo turretInfo = null;

    [HideInInspector] float maxHp;
    [HideInInspector] public float AttackRate;
    [HideInInspector] public float AttackDamage;
    [HideInInspector] public float AttackRange;
    [HideInInspector] public float currentHp;

    private void Start()
    {
        InitializeStats();
    }

    public void InitializeStats()
    {
        maxHp = turretInfo.GetHp();
        AttackRate = turretInfo.GetAttackRate();
        AttackDamage = turretInfo.GetAttackDamage();
        AttackRange = turretInfo.GetAttackRange();
        currentHp = maxHp;
    }

}