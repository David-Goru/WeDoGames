using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a class
/// </summary>
[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade", order = 1)]
public class Upgrade : ScriptableObject
{
    [SerializeField] string description = "Upgrade description";
    [SerializeField] int points = 0;
    Transform objectUI;

    public string Description { get => description; }
    public int Points { get => points; }
    public Transform ObjectUI { get => objectUI; set => objectUI = value; }
}