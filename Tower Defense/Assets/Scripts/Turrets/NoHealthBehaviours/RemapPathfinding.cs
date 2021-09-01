﻿/// <summary>
/// This is a class
/// </summary>
public class RemapPathfinding : TurretNoHealth
{
    BuildingRange buildingRange;

    private void Awake()
    {
        buildingRange = GetComponentInParent<BuildingRange>();
    }

    public override void OnTurretNoHealth()
    {
        Master.Instance.grid.SetWalkableNodes(true, transform.position, buildingRange.Range, transform.parent);
    }

}
