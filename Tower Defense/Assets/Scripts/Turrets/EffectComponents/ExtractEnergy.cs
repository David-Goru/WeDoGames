using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExtractEnergy : EffectComponent
{

    TurretStats turretStats;
    Master master;

    List<ITurretAttackState> attackStateBehaviours = new List<ITurretAttackState>();
    List<ITurretShotBehaviour> shotBehaviours = new List<ITurretShotBehaviour>();

    bool onPlanningPhase = true;
    float timer = 0f;

    private void Start()
    {
        getDependencies();
    }

    public override void InitializeComponent()
    {
        initMembers();
    }

    void getDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
        master = Master.Instance;
        attackStateBehaviours = GetComponents<ITurretAttackState>().ToList();
        shotBehaviours = GetComponents<ITurretShotBehaviour>().ToList();
    }

    public override void UpdateComponent()
    {
        checkIfPlanningPhaseChanged();
        if (!onPlanningPhase)
        {
            if (timer >= turretStats.GetStatValue(StatType.ATTACKSPEED))
            {
                extract();
                timer = 0f;
            }
            else timer += Time.deltaTime;
        }
    }

    void checkIfPlanningPhaseChanged()
    {
        if(onPlanningPhase != Waves.OnPlanningPhase)
        {
            onPlanningPhase = Waves.OnPlanningPhase;
            if (onPlanningPhase) timer = 0f;
            callAttackStateBehaviours(!onPlanningPhase);
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

    void callShotBehaviours()
    {
        foreach (ITurretShotBehaviour shotBehaviour in shotBehaviours) shotBehaviour.OnShot();
    }

    void initMembers()
    {
        onPlanningPhase = true;
        timer = 0f;
    }

    void extract()
    {
        callShotBehaviours();
        master.UpdateBalance(turretStats.GetStatValue(StatType.ENERGYTOEXTRACT));
    }

}
