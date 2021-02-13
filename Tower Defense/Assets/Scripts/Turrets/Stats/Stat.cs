using UnityEditor;
using UnityEngine;

/// <summary>
/// This is a class
/// </summary>

public enum StatType { PRICE, MAXHEALTH, DAMAGE, ATTACKRANGE, ATTACKRATE};

[System.Serializable]
public class Stat
{
    public StatType Type;
    public float Value;

    public Stat(StatType statType, float statValue)
    {
        Type = statType;
        Value = statValue;
    }
}