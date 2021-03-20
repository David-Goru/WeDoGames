using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a class
/// </summary>
[CreateAssetMenu(fileName = "Passive", menuName = "Upgrades/Passive", order = 0)]
public class Passive : Upgrade
{
    [SerializeField] List<Stat> stats;

    public List<Stat> Stats { get => stats; }
}