using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class BuildingRange : MonoBehaviour
{

    [SerializeField] float range;

    public float Range { get => range; set => range = value; }
}