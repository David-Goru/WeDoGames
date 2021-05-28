using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a class
/// </summary>
[CreateAssetMenu(fileName = "ElementalStatUpgrade", menuName = "Upgrades/ElementalStatUpgrade", order = 0)]
public class ElementalStatUpgrade : Upgrade
{
    [SerializeField] TurretElement element = TurretElement.NONE;
    [SerializeField] List<Stat> stats = null;

    public List<Stat> Stats { get => stats; }
    public TurretElement Element { get => element; }
}