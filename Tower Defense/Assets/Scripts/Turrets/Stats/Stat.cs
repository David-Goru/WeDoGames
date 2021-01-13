using UnityEditor;
using UnityEngine;

/// <summary>
/// This is a class
/// </summary>

[System.Serializable]
public class Stat
{
    [HideInInspector] public string StatName;
    [SerializeField] public float StatValue;

    public Stat(string statName, float statValue)
    {
        StatName = statName;
        StatValue = statValue;
    }

    /*void ChangeStatValue(float change)
    {
        stat += change;
    }*/
}
