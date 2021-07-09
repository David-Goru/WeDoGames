using UnityEngine;

public class EntityTurret : Entity
{
    TurretStats turretStats;

    private void Awake()
    {
        turretStats = GetComponent<TurretStats>();
    }

    public override void SetInfo() 
    {
        title = turretStats.name;
        maxHP = Mathf.RoundToInt(turretStats.SearchStatValue(StatType.MAXHEALTH));
        currentHP = Mathf.RoundToInt(turretStats.currentHp);
    }
}