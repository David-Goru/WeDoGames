using UnityEngine;

[CreateAssetMenu(fileName = "HealActiveAction", menuName = "Upgrades/HealActiveActions/HealActiveAction", order = 0)]
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
        Debug.Log("PUM, CURADO");
        int nTurrets = Physics.OverlapSphereNonAlloc(position, 3f, colsCache, objectLayer);
        for (int i = 0; i < nTurrets; i++)
        {
            IHealable turret = colsCache[i].GetComponent<IHealable>();
            if(turret != null)
                turret.Heal(healingValue);
        }
    }
}