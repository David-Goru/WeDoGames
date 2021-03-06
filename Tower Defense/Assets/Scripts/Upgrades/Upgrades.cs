using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a class
/// </summary>
public class Upgrades : MonoBehaviour
{
    //[SerializeField] List<Upgrade> currentUpgrades;

    void Start()
    {
        //currentUpgrades = new List<Upgrade>();
    }

    public void AddUpgrade(Upgrade upgrade)
    {
        // Close UI?
        BuildingInfo[] turrets = MasterHandler.Instance.MasterInfo.GetBuildingsSet();
        //currentUpgrades.Add(upgrade);
        foreach (BuildingInfo turret in turrets)
        {
            foreach (Passive perk in upgrade.Perk)
            {
                turret.IncrementStat(perk.Stat.Type, perk.Stat.Value);
            }
        }
    }
}