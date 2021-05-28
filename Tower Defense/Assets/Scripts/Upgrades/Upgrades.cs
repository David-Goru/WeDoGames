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
        else if (upgrade is Passive)
        {
            Passive passive = (Passive)upgrade;
            BuildingInfo[] turrets = MasterHandler.Instance.MasterInfo.GetTurretsSet();
            foreach (BuildingInfo turret in turrets)
            {
                foreach (Stat stat in passive.Stats)
                {
                    turret.IncrementStat(stat.Type, stat.Value);
                }

                turretsUI.UpdateTurretInfo(turret);
            }
        }
        else if (upgrade is TurretTransformation)
        {
            //Select the correct slot
            //Enter all the turretUpgrades of that turret on the upgradesSet
        }

        upgradesUI.CloseUpgrades();
    }
}