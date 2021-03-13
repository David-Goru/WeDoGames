using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class RemapPathfinding : MonoBehaviour, ITurretNoHealth
{
    BuildingRange buildingRange;

    private void Awake()
    {
        buildingRange = GetComponent<BuildingRange>();
    }

    public void OnTurretNoHealth()
    {
        MasterHandler.Instance.grid.SetWalkableNodes(true, transform.position, buildingRange.Range);
    }

}