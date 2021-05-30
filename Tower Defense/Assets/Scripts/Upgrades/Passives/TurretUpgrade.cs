using UnityEngine;

[CreateAssetMenu(fileName = "TurretUpgrade", menuName = "Upgrades/TurretUpgrade", order = 0)]
public class TurretUpgrade : Upgrade
{
    [SerializeField] TurretTransformation turretToUpgrade = null;
    [SerializeField] Stat[] statChanges = null;
    [SerializeField] int usages = 0;
    int currentUsages = 0;

    public TurretTransformation TurretToUpgrade { get => turretToUpgrade; }
    public Stat[] StatChanges { get => statChanges; }
    public int Usages { get => usages; set => usages = value; }
    public int CurrentUsages { get => currentUsages; set => currentUsages = value; }

    public void UpgradeTurret()
    {
        foreach (Stat stat in statChanges)
        {
            turretToUpgrade.TurretInfo.IncrementStat(stat.Type, stat.Value);
        }
    }
}