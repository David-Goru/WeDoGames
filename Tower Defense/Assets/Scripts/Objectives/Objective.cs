using UnityEngine;

[System.Serializable]
public class Objective : ScriptableObject
{
    [System.NonSerialized] public GameObject UIObject;

    public virtual bool HasBeenCompleted() { return false; }
    public virtual void SetDisplayText() { }
}