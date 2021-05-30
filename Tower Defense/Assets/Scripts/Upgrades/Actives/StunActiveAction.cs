using UnityEngine;

[CreateAssetMenu(fileName = "StunActiveAction", menuName = "Upgrades/ActiveActions/StunActiveAction", order = 0)]
public class StunActiveAction : ActiveAction
{
    Collider[] colsCache = new Collider[32];
    LayerMask enemyLayer;
    [SerializeField] float stunTime = 3.5f;

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
            IStunnable enemy = colsCache[i].GetComponent<IStunnable>();
            if (enemy != null)
                enemy.Stun(stunTime);
        }
    }
}
