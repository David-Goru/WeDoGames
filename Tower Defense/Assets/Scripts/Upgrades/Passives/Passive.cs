using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
/// 
[CreateAssetMenu(fileName = "Passive", menuName = "Upgrades/Passive", order = 0)]
public class Passive : Perk
{
    [SerializeField] Stat stat;

    public Stat Stat { get => stat;}
}