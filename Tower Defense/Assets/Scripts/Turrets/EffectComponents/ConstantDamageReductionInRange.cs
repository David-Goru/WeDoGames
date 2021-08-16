using UnityEngine;

public class ConstantDamageReductionInRange : EffectComponent
{
    const float TIME_INTERVAL = 0.2f; //interval time for not checking enemies all time
    [SerializeField] CurrentTargetsOnRange targetDetection = null;

    TurretStats turretStats;

    float damageReductionDuration;
    float damageReduction;

    float timer = 0f;

    public override void InitializeComponent()
    {
        turretStats = GetComponentInParent<TurretStats>();
    }

    public override void UpdateComponent()
    {
        getStats();
        if (timer >= TIME_INTERVAL)
        {
            reduceDamage();
            timer = 0f;
        }
        else timer += Time.deltaTime;
    }

    void reduceDamage()
    {
        foreach (Transform target in targetDetection.CurrentTargets)
        {
            IDamageReductible damageReductible = target.GetComponent<IDamageReductible>();
            if (damageReductible != null) damageReductible.ReduceDamage(damageReductionDuration, damageReduction);
        }
    }

    void getStats()
    {
        damageReductionDuration = turretStats.GetStatValue(StatType.EFFECTDURATION);
        damageReduction = turretStats.GetStatValue(StatType.DAMAGEREDUCTION);
    }

}