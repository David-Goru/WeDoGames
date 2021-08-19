using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade", order = 1)]
public class Upgrade : ScriptableObject
{
    [SerializeField] string description = "Upgrade description";
    [SerializeField] Sprite icon = null;
    Transform objectUI;

    public string Description { get => description; }
    public Sprite Icon { get => icon; set => icon = value; }
    public Transform ObjectUI { get => objectUI; set => objectUI = value; }
}