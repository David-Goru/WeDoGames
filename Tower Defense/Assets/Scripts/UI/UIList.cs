using UnityEngine;

/// <summary>
/// This is the base for UI lists
/// </summary>
public class UIList : MonoBehaviour
{
    [Tooltip("Object where the UI buildings will be added")]
    public Transform ListUIObject;
    [Tooltip("Prefab with the UI element for every building")]
    public GameObject ObjectUIPrefab;
    public virtual void Initialize(MasterInfo masterInfo, Transform masterObject) { }
}