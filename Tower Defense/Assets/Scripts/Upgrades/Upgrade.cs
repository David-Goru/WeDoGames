using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade", order = 1)]
public class Upgrade : ScriptableObject
{
    [SerializeField] string description = "Upgrade description";
    [SerializeField] int price = 0;
    [SerializeField] Sprite icon = null;
    Transform objectUI;

    public string Description { get => description; }
    public int Price { get => price; }
    public Sprite Icon { get => icon; set => icon = value; }
    public Transform ObjectUI { get => objectUI; set => objectUI = value; }
}