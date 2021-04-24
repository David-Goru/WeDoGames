using UnityEngine;

[CreateAssetMenu(fileName = "SlowActiveAction", menuName = "Upgrades/SlowActiveActions/SlowActiveAction", order = 0)]
public class SlowActiveAction : ActiveAction
{
    Collider[] colsCache = new Collider[32];
    LayerMask objectLayer;
    [SerializeField] float healingValue = 20f;

    private void Awake()
    {
        objectLayer = LayerMask.GetMask("Object");
    }

    public override void UseActive(Vector3 position)
    {
        int nTurrets = Physics.OverlapSphereNonAlloc(position, activeRange, colsCache, objectLayer);
        for (int i = 0; i < nTurrets; i++)
        {
            IHealable turret = colsCache[i].GetComponent<IHealable>();
            if(turret != null)
                turret.Heal(healingValue);
        }
    }
}
