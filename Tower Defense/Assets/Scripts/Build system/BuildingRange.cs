using UnityEngine;

/// <summary>
/// Used on the building system for updating the grid with unwalkable nodes within this range.
/// </summary>
public class BuildingRange : MonoBehaviour
{
    [SerializeField] float range;

    public float Range { get => range; set => range = value; }
}