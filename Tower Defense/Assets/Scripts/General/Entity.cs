using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class Entity : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] protected string title = "";
    [SerializeField] protected int currentHP = 0;

    [Header("Attributes")]
    [SerializeField] protected int maxHP = 0;

    public string Title { get => title; }
    public int CurrentHP { get => currentHP; }
    public int MaxHP { get => maxHP; }
    public bool IsDead { get => currentHP <= 0; }

    public virtual string GetExtraInfo() { return ""; }

    private void OnMouseDown()
    {
        EntityInfoUI.Instance.ShowUI(this);
    }
}