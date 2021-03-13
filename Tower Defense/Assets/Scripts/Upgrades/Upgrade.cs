using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a class
/// </summary>
[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade", order = 1)]
public class Upgrade : ScriptableObject
{
    public string Description;
    public int points;
    public List<Perk> Perk;
}