using UnityEngine;

[CreateAssetMenu(fileName = "FearActiveAction", menuName = "Upgrades/FearActiveActions/FearActiveAction", order = 0)]
public class FearActiveAction : ActiveAction
{
    Collider[] colsCache = new Collider[32];
    LayerMask enemyLayer;
    [SerializeField] float fearTime = 3.5f;

    private void Awake()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    public override void UseActive(Vector3 position)
    {
        if (enemyLayer == 0)
            enemyLayer = LayerMask.GetMask("Enemy");
        int nTurrets = Physics.OverlapSphereNonAlloc(position, activeRange, colsCache, enemyLayer);
        for (int i = 0; i < nTurrets; i++)
        {
            IFearable enemy = colsCache[i].GetComponent<IFearable>();
            if (enemy != null)
                enemy.Fear(fearTime);
        }
    }
}
