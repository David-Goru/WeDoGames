using UnityEngine;

[CreateAssetMenu(fileName = "SlowActiveAction", menuName = "Upgrades/SlowActiveActions/SlowActiveAction", order = 0)]
public class SlowActiveAction : ActiveAction
{
    Collider[] colsCache = new Collider[32];
    LayerMask enemyLayer;
    [SerializeField] float slowTime = 3.5f;

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
            ISlowable enemy = colsCache[i].GetComponent<ISlowable>();
            if (enemy != null)
                enemy.Slow(slowTime, 0.5f);
        }
    }
}
