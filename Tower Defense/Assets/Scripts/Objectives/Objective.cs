using UnityEngine;

[System.Serializable]
public class Objective : ScriptableObject
{
    [System.NonSerialized] public GameObject UIObject;
    public bool Completed = false;

    public virtual void UpdateCompleteState() { }
    public virtual void SetDisplayText() { }
}