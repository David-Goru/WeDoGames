using UnityEngine;

[CreateAssetMenu(fileName = "HealActiveAction", menuName = "Upgrades/ActiveActions/HealActiveAction", order = 0)]
public class HealActiveAction : ActiveAction
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
        if (objectLayer == 0)
            objectLayer = LayerMask.GetMask("Object");
        int nTurrets = Physics.OverlapSphereNonAlloc(position, activeRange, colsCache, objectLayer);
        for (int i = 0; i < nTurrets; i++)
        {
            IHealable turret = colsCache[i].GetComponent<IHealable>();
            if(turret != null)
                turret.Heal(healingValue);
        }
    }
}