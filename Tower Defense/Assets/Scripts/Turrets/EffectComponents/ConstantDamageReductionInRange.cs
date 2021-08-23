using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConstantDamageReductionInRange : EffectComponent
{
    const float TIME_INTERVAL = 0.2f; //interval time for not checking enemies all time
    [SerializeField] CurrentTargetsOnRange targetDetection = null;

    List<ITurretAttackState> attackStateBehaviours = new List<ITurretAttackState>();
    TurretStats turretStats;

    float damageReductionDuration;
    float damageReduction;

    float timer = 0f;
    bool areTargetsInRange = false;

    private void Awake()
    {
        getDependencies();
    }

    public override void InitializeComponent()
    {
        areTargetsInRange = false;
    }

    private void getDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
        attackStateBehaviours = GetComponents<ITurretAttackState>().ToList();
    }

    public override void UpdateComponent()
    {
        getStats();
        checkIfAreTargetsInRangeHasChanged();
        if (timer >= TIME_INTERVAL)
        {
            reduceDamage();
            timer = 0f;
        }
        else timer += Time.deltaTime;
    }

    void checkIfAreTargetsInRangeHasChanged()
    {
        if (areTargetsInRange != targetDetection.AreTargetsInRange)
        {
            areTargetsInRange = targetDetection.AreTargetsInRange;
            callAttackStateBehaviours(areTargetsInRange);
        }
    }

    void callAttackStateBehaviours(bool enter)
    {
        foreach (ITurretAttackState attackStateBehaviour in attackStateBehaviours)
        {
            if (enter) attackStateBehaviour.OnAttackEnter();
            else attackStateBehaviour.OnAttackExit();
        }
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