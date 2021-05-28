using UnityEngine;

[CreateAssetMenu(fileName = "TurretTransformation", menuName = "Upgrades/TurretTransformation", order = 0)]
public class TurretTransformation : Upgrade
{
    [SerializeField] Pool turret = null;
    [SerializeField] TurretTier turretTier = TurretTier.FIRST;
    [SerializeField] TurretElement turretElement = TurretElement.NONE;
    [SerializeField] BuildingInfo turretInfo = null;
    [SerializeField] BuildingInfo parentTurretInfo = null;

    public Pool Turret { get => turret; }
    public TurretTier TurretTier { get => turretTier; set => turretTier = value; }
    public TurretElement TurretElement { get => turretElement; }
    public BuildingInfo TurretInfo { get => turretInfo; set => turretInfo = value; }
    public BuildingInfo ParentTurretInfo { get => parentTurretInfo; }
}