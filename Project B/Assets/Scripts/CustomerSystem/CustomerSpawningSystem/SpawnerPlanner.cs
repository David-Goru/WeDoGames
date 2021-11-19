using System;
using UnityEngine;

public class SpawnerPlanner : MonoBehaviour, IPlannerMethodsInstaller, IPlannerReceiverInstaller
{
    ISpawnerPlannerMethod[] plannerMethods;
    ISpawnerPlannerReceiver plannerReceiver;

    Action spawnAction;

    public void SetPlannerMethods(ISpawnerPlannerMethod[] plannerMethods)
    {
        this.plannerMethods = plannerMethods;
        if (spawnAction == null) spawnAction = onSpawn;
        foreach (var plannerMethod in plannerMethods)
        {
            plannerMethod.SetSpawnerEvent(spawnAction);
        }
    }

    public void SetSpawnerReceiver(ISpawnerPlannerReceiver plannerReceiver)
    {
        this.plannerReceiver = plannerReceiver;
    }

    void onSpawn()
    {
        plannerReceiver.OnSpawn();
    }
}
