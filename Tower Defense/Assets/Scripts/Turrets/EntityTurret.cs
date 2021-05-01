using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class EntityTurret : Entity
{
    TurretStats turretStats;
    const string info = @"Attack Range: {0:0.##}
Attack Rate: {1:0.##}
Attack Damage: {2:0.##}";

    private void Awake()
    {
        turretStats = GetComponent<TurretStats>();
    }

    public override string GetExtraInfo() 
    {
        maxHP = Mathf.RoundToInt(turretStats.SearchStatValue(StatType.MAXHEALTH));
        currentHP = Mathf.RoundToInt(turretStats.currentHp);

        return string.Format(info, turretStats.SearchStatValue(StatType.ATTACKRANGE), turretStats.SearchStatValue(StatType.ATTACKRATE),
            turretStats.SearchStatValue(StatType.DAMAGE));
    }
}