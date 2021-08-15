using UnityEngine;

/// <summary>
/// Interface for slowing an enemy
/// </summary>
public interface ISlowable
{
    void Slow(float secondsSlowed, float slowReduction);
}
