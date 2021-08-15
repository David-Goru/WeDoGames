using System;
using UnityEngine;

public class ExtractEnergy : EffectComponent
{
    [SerializeField] Animator anim = null;
    [SerializeField] AudioSource audioSource = null;

    TurretStats turretStats;
    Master master;

    bool onPlanningPhase = true;
    float timer = 0f;

    public override void InitializeComponent()
    {
        getDependencies();
        initMembers();
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
        if(onPlanningPhase != Waves.onPlanningPhase)
        {
            onPlanningPhase = Waves.onPlanningPhase;
            if (onPlanningPhase)
            {
                setAnimationState(false);
                timer = 0f;
            }
            else setAnimationState(true);
        }
    }

    void getDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
        master = Master.Instance;
    }

    void initMembers()
    {
        onPlanningPhase = true;
        timer = 0f;
        setAnimationState(false);
    }

    void extract()
    {
        master.UpdateBalance(turretStats.GetStatValue(StatType.ENERGYTOEXTRACT));
    }

    void setAnimationState(bool isExtracting)
    {
        if (anim != null) anim.SetBool("IsExtracting", isExtracting);
    }
    /*
    void playSound()
    {
        if (audioSource != null) audioSource.Play();
    }*/
}
