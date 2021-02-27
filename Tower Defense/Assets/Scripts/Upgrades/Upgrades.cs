using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a class
/// </summary>
public class Upgrades : MonoBehaviour
{
    [SerializeField] List<Upgrade> currentUpgrades;

    void Start()
    {
        currentUpgrades = new List<Upgrade>();
    }

    public void AddUpgrade(Upgrade upgrade)
    {
        // Close UI?
        currentUpgrades.Add(upgrade);
    }
}