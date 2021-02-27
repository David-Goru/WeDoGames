using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a class
/// </summary>
public class Upgrades : MonoBehaviour
{
    [SerializeField] List<Upgrade> currentUpgrades;

    public static Upgrades Instance;

    void Start()
    {
        Instance = this;
    }

    public void AddUpgrade(Upgrade upgrade)
    {
        currentUpgrades.Add(upgrade);
    }
}