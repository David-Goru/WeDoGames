using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
[CreateAssetMenu(fileName = "TestActiveAction", menuName = "Upgrades/ActiveActions/TestActiveAction", order = 0)]
public class TestActiveAction : ActiveAction
{
    public override void UseActive(Vector3 position)
    {
        Debug.Log("Hellooooooooooooooooooooooou");
    }
}

[CreateAssetMenu(fileName = "HealActiveAction", menuName = "Upgrades/HealActiveActions/HealActiveAction", order = 0)]
public class HealActiveAction : ActiveAction
{
    bool isActive;

    public override void UseActive(Vector3 position)
    {
        isActive = true;
    }
}