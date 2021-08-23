using UnityEngine;

public interface IKnockbackable
{
    void Knockback(float knockbackDistance, Vector3 knockbackDirection);
}