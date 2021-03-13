using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a class
/// </summary>
public class Upgrades : MonoBehaviour
{
    [SerializeField] BuildingsUI buildingsUI = null;

    public void AddUpgrade(Upgrade upgrade)
    {
        BuildingInfo[] turrets = MasterHandler.Instance.MasterInfo.GetBuildingsSet();
        foreach (BuildingInfo turret in turrets)
        {
            foreach (Passive perk in upgrade.Perk)
            {
                turret.IncrementStat(perk.Stat.Type, perk.Stat.Value);
            }

            buildingsUI.UpdateBuildingInfo(turret);
        }
    }
}