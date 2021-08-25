using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "ScriptableObjects/EnemyInfo", order = 0)]
public class EnemyInfo : ScriptableObject
{
    [SerializeField] int maxHealth = 0;
    [SerializeField] int damage = 0;
    [SerializeField] int coinsReward = 0;
    [SerializeField] float attackSpeed = 0f;
    [SerializeField] float range = 0f;
    [SerializeField] float defaultSpeed = 0f;
    [SerializeField] float initRotationSpeed = 0f;

    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Damage { get => damage; set => damage = value; }
    public int CoinsReward { get => coinsReward; set => coinsReward = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float Range { get => range; set => range = value; }
    public float DefaultSpeed { get => defaultSpeed; set => defaultSpeed = value; }
    public float InitRotationSpeed { get => initRotationSpeed; set => initRotationSpeed = value; }
}