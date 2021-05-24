using UnityEngine;

[CreateAssetMenu(fileName = "TurretUpgrade", menuName = "Upgrades/TurretUpgrade", order = 0)]
public class TurretUpgrade : Upgrade
{
    TurretInfo turretToUpgrade;
    [SerializeField] Stat[] statIncrements = null;

    public Stat[] StatChanges { get => statIncrements; }

    public void SetTurret(TurretInfo turretInfo)
    {
        turretToUpgrade = turretInfo;
    }

    public void UpgradeTurret()
    {
        foreach (Stat stat in statIncrements)
        {
            turretToUpgrade.IncrementStat(stat.Type, stat.Value);
        }
    }
}
