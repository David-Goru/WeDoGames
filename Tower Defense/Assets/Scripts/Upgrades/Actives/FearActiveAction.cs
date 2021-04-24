using UnityEngine;

[CreateAssetMenu(fileName = "FearActiveAction", menuName = "Upgrades/FearActiveActions/FearActiveAction", order = 0)]
public class FearActiveAction : ActiveAction
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
