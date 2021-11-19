using UnityEngine;

public class SpawningSystemInstaller : MonoBehaviour, ILoadable
{
    IPlannerMethodsInstaller plannerMethodsInstaller;
    IPlannerReceiverInstaller plannerReceiverInstaller;
    ISpawnerInstaller spawnerInstaller;

    ISpawnerPlannerMethod[] spawnerPlannerMethods;
    ISpawnerPlannerReceiver spawnerPlannerReceiver;
    ISpawnerPositionMethod spawnerPositionMethod;
    INPCSelectorMethod npcSelectorMethod;
    ISpawnerMethod spawnerMethod;

    public void Create()
    {
        getDependencies();
        setDependencies();
    }

    void getDependencies()
    {
        Transform parent = transform.parent;
        plannerMethodsInstaller = parent.GetComponentInChildren<IPlannerMethodsInstaller>();
        plannerReceiverInstaller = parent.GetComponentInChildren<IPlannerReceiverInstaller>();
        spawnerInstaller = parent.GetComponentInChildren<ISpawnerInstaller>();
        spawnerPlannerMethods = parent.GetComponentsInChildren<ISpawnerPlannerMethod>();
        spawnerPlannerReceiver = parent.GetComponentInChildren<ISpawnerPlannerReceiver>();
        spawnerPositionMethod = parent.GetComponentInChildren<ISpawnerPositionMethod>();
        npcSelectorMethod = parent.GetComponentInChildren<INPCSelectorMethod>();
        spawnerMethod = parent.GetComponentInChildren<ISpawnerMethod>();
    }

    void setDependencies()
    {
        plannerMethodsInstaller.SetPlannerMethods(spawnerPlannerMethods);
        plannerReceiverInstaller.SetSpawnerReceiver(spawnerPlannerReceiver);
        spawnerInstaller.SetSpawnerPositionMethod(spawnerPositionMethod);
        spawnerInstaller.SetNPCSelectorMethod(npcSelectorMethod);
        spawnerInstaller.SetSpawnerMethod(spawnerMethod);
    }
}
