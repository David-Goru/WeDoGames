using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a class
/// </summary>
public class Upgrades : MonoBehaviour
{
    [SerializeField] TurretsUI turretsUI = null;
    [SerializeField] ActivesUI activesUI = null;
    UpgradesUI upgradesUI;

    public UpgradesUI UpgradesUI { get => upgradesUI; set => upgradesUI = value; }

    public void AddUpgrade(Upgrade upgrade)
    {
        if (upgrade is Active)
        {
            activesUI.EnableActive(upgrade);
        }
        else if (upgrade is ElementalStatUpgrade)
        {
            ElementalStatUpgrade elementalStatUpgrade = (ElementalStatUpgrade)upgrade;
            BuildingInfo[] turrets = MasterHandler.Instance.MasterInfo.GetTurretsSet();
            foreach (BuildingInfo turret in turrets)
            {
                if (turret.TurretElement == elementalStatUpgrade.Element)
                {
                    foreach (Stat stat in elementalStatUpgrade.Stats)
                    {
                        turret.IncrementStat(stat.Type, stat.Value);
                    }
                }
            }

            turretsUI.UpdateTurretElement(elementalStatUpgrade.Element);
        }
        else if (upgrade is TurretTransformation)
        {
            turretsUI.AddTurretUpgrade((TurretTransformation)upgrade);
        }
        else if (upgrade is TurretUpgrade)
        {
            TurretUpgrade turretUpgrade = (TurretUpgrade)upgrade;
            turretUpgrade.CurrentUsages++;

            foreach (Stat stat in turretUpgrade.StatChanges)
            {
                turretUpgrade.TurretToUpgrade.TurretInfo.IncrementStat(stat.Type, stat.Value);
            }

            turretsUI.UpdateTurretInfo(turretUpgrade.TurretToUpgrade.TurretInfo);
        }

        upgradesUI.CloseUpgrades();
    }
}