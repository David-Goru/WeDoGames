using UnityEngine;

[System.Serializable]
public class FurnitureData
{
    [SerializeField] private string name;
    [SerializeField] private bool buildableAboveFloor;
    [SerializeField] private bool buildableAboveFurniture;
    
    public string Name => name;
    public bool BuildableAboveFloor => buildableAboveFloor;
    public bool BuildableAboveFurniture => buildableAboveFurniture;
}