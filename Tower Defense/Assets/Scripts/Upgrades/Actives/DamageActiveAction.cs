using UnityEngine;

[CreateAssetMenu(fileName = "DamageActiveAction", menuName = "Upgrades/DamageActiveActions/DamageActiveAction", order = 0)]
public class DamageActiveAction : ActiveAction
{
    Collider[] colsCache = new Collider[128];
    LayerMask enemyLayer;
    [SerializeField] float damageValue = 20f;

    private void Awake()
    {
        Debug.Log("Hola");
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    public override void UseActive(Vector3 position)
    {
        if(enemyLayer == 0)
            enemyLayer = LayerMask.GetMask("Enemy");
        int nEnemies = Physics.OverlapSphereNonAlloc(position, activeRange, colsCache, enemyLayer);
        Debug.Log(nEnemies);
        for (int i = 0; i < nEnemies; i++)
        {
            ITurretDamage enemy = colsCache[i].GetComponent<ITurretDamage>();
            Debug.Log(enemy);
            //if(enemy != null)
                //enemy.OnTurretHit()
        }
    }
}