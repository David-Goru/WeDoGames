using UnityEngine;

public class Nexus : Entity
{
    const string textUI = "Evil beings try to get you out of the planet. But don't worry, your Nexus will keep' em entertained while you pium pium them.";

    public bool IsFullHealth { get => currentHP == maxHP; }
    public bool IsAlive { get => currentHP > 0; }

    public static Nexus Instance;
    public static Transform GetTransform { get => Instance.transform; }

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        currentHP = maxHP;
    }

    public override string GetExtraInfo()
    {
        return textUI;
    }

    public void GetHit(float damage)
    {
        currentHP -= Mathf.RoundToInt(damage);
    }
}