using UnityEngine;

public class Furniture : MonoBehaviour
{
    [SerializeField] private FurnitureData data;

    public FurnitureData Data => data;
}