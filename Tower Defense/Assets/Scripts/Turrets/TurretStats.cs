using UnityEngine;

/// <summary>
/// This class stores all the stats of the turret
/// </summary>
public class TurretStats : MonoBehaviour
{
    [SerializeField] TurretInfo turretInfo = null;

    float maxHp;
    public float AttackRate;
    public float AttackDamage;
    public float AttackRange;
    public float currentHp;

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