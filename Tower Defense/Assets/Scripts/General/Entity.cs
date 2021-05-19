using UnityEngine;

/// <summary>
/// Base entity class for everything that has HP and can be clickable. Displays title and HP on click to the UI.
/// </summary>
public class Entity : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] protected string title = "";
    [SerializeField] protected int currentHP = 0;
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