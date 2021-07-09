using UnityEngine;

[CreateAssetMenu(fileName = "DamageActiveAction", menuName = "Upgrades/ActiveActions/DamageActiveAction", order = 0)]
public class DamageActiveAction : ActiveAction
{
    Collider[] colsCache = new Collider[128];
    LayerMask enemyLayer;
    [SerializeField] int damageValue = 20;

    private void Awake()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    public override void UseActive(Vector3 position)
    {
        if(enemyLayer == 0) enemyLayer = LayerMask.GetMask("Enemy");
        int nEnemies = Physics.OverlapSphereNonAlloc(position, activeRange, colsCache, enemyLayer);
        for (int i = 0; i < nEnemies; i++)
        {
            IDamageable enemy = colsCache[i].GetComponent<IDamageable>();
            if (enemy != null) enemy.GetDamage(damageValue);
        }
    }
}