using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class EnemiesUI : Entity
{
    private Base_AI enemy;
    const string info = @"Attack Range: {0:0.##}
Attack Rate: {1:0.##}
Attack Damage: {2:0.##}
Speed: {3:0.##}";

    private void Awake()
    {
        enemy = GetComponent<Base_AI>();
    }

    public override string GetExtraInfo()
    {
        maxHP = Mathf.RoundToInt(enemy.GetMaxHealth);
        currentHP = Mathf.RoundToInt(enemy.GetHealth);

        return string.Format(info, enemy.Range, enemy.AttackSpeed, enemy.Damage, enemy.GetSpeed);
    }
}