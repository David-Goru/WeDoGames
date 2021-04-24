using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class Entity : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private string title = "";
    [SerializeField] private int currentHP = 0;
    [SerializeField] private int maxHP = 0;

    public string Title { get => title; }
    public int CurrentHP { get => currentHP; }
    public int MaxHP { get => maxHP; }

    public virtual string GetExtraInfo() { return ""; }

    private void OnMouseDown()
    {
        EntityInfoUI.Instance.ShowUI(this);
    }
}