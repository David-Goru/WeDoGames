using UnityEngine;

public class EntityNexus : Entity
{
    Nexus nexus;
    const string info = "Evil beings try to get you out of the planet. But don't worry, your Nexus will keep' em entertained while you pium pium them.";

    private void Awake()
    {
        nexus = GetComponent<Nexus>();
    }

    public override string GetExtraInfo()
    {
        maxHP = Mathf.RoundToInt(nexus.MaxHealth);
        currentHP = Mathf.RoundToInt(nexus.Health);

        return info;
    }
}