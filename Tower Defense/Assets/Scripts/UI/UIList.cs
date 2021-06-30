using UnityEngine;

public class UIList : MonoBehaviour
{
    [Tooltip("Object where the UI objects will be added")] public Transform ListUIObject;
    [Tooltip("Prefab with the UI element for every object")] public GameObject ObjectUIPrefab;
    public virtual void Initialize(MasterInfo masterInfo, Transform masterObject) { }
}