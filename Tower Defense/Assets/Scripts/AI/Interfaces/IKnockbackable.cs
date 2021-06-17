using UnityEngine;

/// <summary>
/// Interface for knockback enemies
/// </summary>
public interface IKnockbackable
{
    void Knockback(float knockbackDistance);
}