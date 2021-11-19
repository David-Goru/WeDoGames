using UnityEngine;

public class Spawner : MonoBehaviour, ISpawnerPlannerReceiver, ISpawnerInstaller
{
    INPCSelectorMethod npcSelectorMethod;
    ISpawnerMethod spawnerMethod;
    ISpawnerPositionMethod spawnerPositionMethod;

    public void SetNPCSelectorMethod(INPCSelectorMethod npcSelectorMethod)
    {
        this.npcSelectorMethod = npcSelectorMethod;
    }

    public void SetSpawnerMethod(ISpawnerMethod spawnerMethod)
    {
        this.spawnerMethod = spawnerMethod;
    }

    public void SetSpawnerPositionMethod(ISpawnerPositionMethod spawnerPositionMethod)
    {
        this.spawnerPositionMethod = spawnerPositionMethod;
    }

    public void OnSpawn()
    {
        NPCData npcData = npcSelectorMethod.GetNpc();
        Vector3 spawnPosition = spawnerPositionMethod.GetSpawnPosition();
        npcData.position = spawnPosition;
        spawnerMethod.Spawn(npcData);
    }

}
