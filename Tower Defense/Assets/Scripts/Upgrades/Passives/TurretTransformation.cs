using UnityEngine;

[CreateAssetMenu(fileName = "TurretTransformation", menuName = "Upgrades/TurretTransformation", order = 0)]
public class TurretTransformation : Upgrade
{
    [SerializeField] Pool turret = null;
    [SerializeField] bool isBasic = false;
    [SerializeField] TurretElement turretElement = TurretElement.NONE;

    public Pool Turret { get => turret; }
    public bool IsBasic { get => isBasic; }
    public TurretElement TurretElement { get => turretElement; }
}
