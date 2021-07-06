using UnityEngine;

[CreateAssetMenu(fileName = "TurretTransformation", menuName = "Upgrades/TurretTransformation", order = 0)]
public class TurretTransformation : Upgrade
{
    [SerializeField] Pool turret = null;
    [SerializeField] TurretTier turretTier = TurretTier.FIRST;
    [SerializeField] TurretElement turretElement = TurretElement.NONE;
    [SerializeField] TurretInfo turretInfo = null;
    [SerializeField] TurretInfo parentTurretInfo = null;

    public Pool Turret { get => turret; }
    public TurretTier TurretTier { get => turretTier; set => turretTier = value; }
    public TurretElement TurretElement { get => turretElement; }
    public TurretInfo TurretInfo { get => turretInfo; set => turretInfo = value; }
    public TurretInfo ParentTurretInfo { get => parentTurretInfo; }
}