using UnityEngine;

public class Entity : MonoBehaviour
{
    protected string title = "";
    protected int currentHP = 0;
    protected int maxHP = 0;

    public string Title { get => title; set => title = value; }
    public int CurrentHP { get => currentHP; set => currentHP = value; }
    public int MaxHP { get => maxHP; set => maxHP = value; }

    private void OnMouseDown()
    {
        if (Time.timeScale != 0) UI.ShowEntityInfoUI(this);
    }

    public bool IsDead { get => currentHP <= 0; }
}