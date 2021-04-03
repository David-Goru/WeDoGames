using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
[CreateAssetMenu(fileName = "Active action", menuName = "Upgrades/ActiveAction", order = 0)]
public class ActiveAction : ScriptableObject
{
    public virtual void UseActive(Vector3 hitPos) { }
    [HideInInspector] public Transform UIElement;
}
