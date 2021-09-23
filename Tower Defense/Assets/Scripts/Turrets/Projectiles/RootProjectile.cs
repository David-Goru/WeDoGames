using UnityEngine;

public class RootProjectile : Projectile
{
    [SerializeField] MeshRenderer mesh;
    [SerializeField] float blendSpeed;
    Material material;

    float damage = 0;
    float damageTimer = 0f;
    float lifeTimer = 0f;
    float damageInterval = 0f;
    float lifeTime = 0f;
    float blending = 0f;
    ITurretDamage turretDamage = null;

    void Awake()
    {
        material = mesh.material;
    }

    protected override void initializate()
    {
        damageTimer = 0f;
        lifeTimer = 0f;
        damage = turretStats.GetStatValue(StatType.DAMAGE);
        damageInterval = 1 / damage;
        lifeTime = turretStats.GetStatValue(StatType.EFFECTDURATION);
        blending = 0f;
        turretDamage = target.GetComponent<ITurretDamage>();
        material.SetFloat("Grow", blending);
        target.GetComponent<IStunnable>().Stun(turretStats.GetStatValue(StatType.EFFECTDURATION));
    }

    protected override void updateProjectile()
    {
        if (!target.gameObject.activeSelf || lifeTimer >= lifeTime) disable();
        transform.position = target.position;
        blending = Mathf.Lerp(blending, 1f, blendSpeed * Time.deltaTime);
        material.SetFloat("Grow", blending);
        if (damageTimer >= damageInterval)
        {
            doDamage();
            damageTimer = 0f;
        }
        else damageTimer += Time.deltaTime;
        lifeTimer += Time.deltaTime;
    }

    void doDamage()
    {
        if (turretDamage != null) turretDamage.OnTurretHit(turret, 1, enemyDamageHandler);
    }
}