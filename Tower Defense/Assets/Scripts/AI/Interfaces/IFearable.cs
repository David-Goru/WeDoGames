using UnityEngine;

/// <summary>
/// Interface for fearing an enemy
/// </summary>
public interface IFearable
{
    void Fear(float fearSeconds);
}