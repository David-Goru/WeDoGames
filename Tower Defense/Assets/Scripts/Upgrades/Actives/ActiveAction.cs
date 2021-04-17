using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
[CreateAssetMenu(fileName = "Active action", menuName = "Upgrades/ActiveAction", order = 0)]
public class ActiveAction : ScriptableObject
{
    [HideInInspector] public Transform UIElement;
    public float activeCooldown = 30f;

    public virtual void UseActive(Vector3 hitPos) 
    {
        
    }
}
