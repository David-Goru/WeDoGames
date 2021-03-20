using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a class
/// </summary>
public class Upgrades : MonoBehaviour
{
    [SerializeField] BuildingsUI buildingsUI = null;
    [SerializeField] ActivesUI activesUI = null;

    public void AddUpgrade(Upgrade upgrade)
    {
        if (upgrade is Passive)
        {
            Passive passive = (Passive)upgrade;
            BuildingInfo[] turrets = MasterHandler.Instance.MasterInfo.GetBuildingsSet();
            foreach (BuildingInfo turret in turrets)
            {
                foreach (Stat stat in passive.Stats)
                {
                    turret.IncrementStat(stat.Type, stat.Value);
                }

                buildingsUI.UpdateBuildingInfo(turret);
            }
        }
        else if (upgrade is Active)
        {
            activesUI.EnableActive(upgrade);
        }
    }
}