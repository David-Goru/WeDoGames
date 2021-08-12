using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waves : MonoBehaviour
{
    public static int EnemiesRemaining = 0;

    [SerializeField] WavesInfo wavesInfo = null;
    [SerializeField] Objectives objectivesInfo = null;
    [SerializeField] Transform spawners = null;
    [SerializeField] Image[] signals = null;
    [SerializeField] Transform objectivesList = null;
    [SerializeField] GameObject objectivePrefab = null;
    [SerializeField] AudioClip waveStartSound = null;
    [SerializeField] AudioClip waveFinishSound = null;

    List<Vector3> spawnersPositions; 
    int currentWave = 0;
    int waveIndex = 0;
    float timer = 0f;
    bool onPlanningPhase = true;
    ObjectPooler objectPooler;

    void Start()
    {
        spawnersPositions = new List<Vector3>();
        foreach (Transform spawner in spawners) spawnersPositions.Add(spawner.position);

        objectPooler = ObjectPooler.GetInstance();
        UI.UpdateWaveText(currentWave);
        setSignalsVisuals(true);
    }

    void Update()
    {
        updateWaveState();
    }

    void setUpObjectives()
    {
        if (currentWave != 0) removeCurrentObjectives();

        int objectivesIndex = currentWave;
        if (currentWave >= objectivesInfo.ObjectivesOrder.Length) objectivesIndex = objectivesInfo.ObjectivesOrder.Length - 1;

        foreach (Objective objective in objectivesInfo.ObjectivesOrder[objectivesIndex].Objectives)
        {
            objective.UIObject = createObjectiveUI();
            objective.SetDisplayText();
        }
    }

    void removeCurrentObjectives()
    {
        foreach (Objective objective in objectivesInfo.ObjectivesOrder[currentWave - 1].Objectives)
        {
            if (objective.UIObject != null) Destroy(objective.UIObject);
        }
    }

    GameObject createObjectiveUI()
    {
        GameObject uiObject = Instantiate(objectivePrefab);
        uiObject.transform.SetParent(objectivesList, false);

        return uiObject;
    }

    void checkObjectives()
    {
        int objectivesIndex = currentWave;
        if (currentWave >= objectivesInfo.ObjectivesOrder.Length) objectivesIndex = objectivesInfo.ObjectivesOrder.Length - 1;

        foreach (Objective objective in objectivesInfo.ObjectivesOrder[objectivesIndex].Objectives)
        {
            if (objective.HasBeenCompleted()) Debug.Log(objective.name + " completed!");
        }
    }

    void updateWaveState()
    {
        if (onPlanningPhase)
        {
            if (planningPhaseFinished()) startWave();
            else updatePlanningPhase();
        }
        else
        {
            if (currentWaveHasFinished()) endWave();
            else updateCurrentWave();
        }   
    }

    bool planningPhaseFinished()
    {
        return timer > wavesInfo.PlanningTime;
    }

    bool currentWaveHasFinished()
    {
        return EnemiesRemaining == 0;
    }

    void startWave()
    {
        currentWave++;
        timer = 0;
        onPlanningPhase = false;
        spawnEnemies();
        setSignalsVisuals(false);
        setUpObjectives();
        if (waveIndex < wavesInfo.EnemyWaves.Count - 1) waveIndex++;
        UI.UpdateWaveText(currentWave);
        UI.CloseUpgrades();
        UI.UpdateWaveTimerText(Mathf.RoundToInt(0));
        Master.Instance.RunSound(waveStartSound);
    }

    void updateCurrentWave()
    {
        timer += Time.deltaTime;
        UI.UpdateWaveTimerText(Mathf.RoundToInt(timer));
    }

    void updatePlanningPhase()
    {
        UI.UpdateWaveTimerText(Mathf.RoundToInt(wavesInfo.PlanningTime - timer));
        timer += Time.deltaTime;
    }

    void endWave()
    {
        timer = 0;
        onPlanningPhase = true;
        Master.Instance.RunSound(waveFinishSound);
        setSignalsVisuals(true);
        UI.OpenUpgrades(3);
        checkObjectives();
    }

    void spawnEnemies()
    {
        for (int i = 0; i < wavesInfo.EnemyWaves[waveIndex].EnemyWave.Count; i++)
        {
            List<int> possibleSpawners = getRandomSpawnerNum(i);
            for (int j = 0; j < wavesInfo.EnemyWaves[waveIndex].EnemyWave[i].NumberOfEnemies; j++)
            {
                int getRandomSpawn = possibleSpawners[Random.Range(0, possibleSpawners.Count)];
                Vector3 randomPos = spawnersPositions[getRandomSpawn] + Vector3.forward * Random.Range(-3f, 3f) + Vector3.right * Random.Range(-3f, 3f);
                objectPooler.SpawnObject(wavesInfo.EnemyWaves[waveIndex].EnemyWave[i].Enemy.tag, randomPos, Quaternion.Euler(0, 0, 0));
                EnemiesRemaining++;
            }
        }
    }

    void setSignalsVisuals(bool newState)
    {
        foreach (EnemyPool enemyPool in wavesInfo.EnemyWaves[waveIndex].EnemyWave)
        {
            changeSignalsState(enemyPool, newState);
        }
    }

    List<int> getRandomSpawnerNum(int index)
    {
        List<int> possibleSpawners = new List<int>();
        if (wavesInfo.EnemyWaves[waveIndex].EnemyWave[index].DOWN) possibleSpawners.Add((int)WaveDirection.DOWN);
        if (wavesInfo.EnemyWaves[waveIndex].EnemyWave[index].RIGHT) possibleSpawners.Add((int)WaveDirection.RIGHT);
        if (wavesInfo.EnemyWaves[waveIndex].EnemyWave[index].LEFT) possibleSpawners.Add((int)WaveDirection.LEFT);
        if (wavesInfo.EnemyWaves[waveIndex].EnemyWave[index].UP) possibleSpawners.Add((int)WaveDirection.UP);

        if (possibleSpawners.Count == 0) possibleSpawners.Add((int)WaveDirection.DOWN);
        return possibleSpawners;
    }

    void changeSignalsState(EnemyPool enemyPool, bool newState)
    {
        if (enemyPool.DOWN) changeSignalState((int)WaveDirection.DOWN, newState);
        if (enemyPool.RIGHT) changeSignalState((int)WaveDirection.RIGHT, newState);
        if (enemyPool.LEFT) changeSignalState((int)WaveDirection.LEFT, newState);
        if (enemyPool.UP) changeSignalState((int)WaveDirection.UP, newState);
    }

    void changeSignalState(int signalIndex, bool newState)
    {
        signals[signalIndex].gameObject.SetActive(newState);
    }

    public bool WaveCompletedInLessThan(float time)
    {
        return timer <= time;
    }

    public static void KillEnemy()
    {
        EnemiesRemaining--;
    }
}

[System.Serializable]
public class EnemyList
{
    public List<EnemyPool> EnemyWave;

    public EnemyList(List<EnemyPool> enemyWave)
    {
        EnemyWave = enemyWave;
    }
}

[System.Serializable]
public class EnemyPool
{
    [Header("Wave enemies")]
    public Pool Enemy;
    public int NumberOfEnemies;

    [Header("Spawn Positions")]
    public bool DOWN;
    public bool RIGHT;
    public bool LEFT;
    public bool UP;

    public EnemyPool(Pool enemy, int numberOfEnemies, bool up, bool down, bool left, bool right)
    {
        Enemy = enemy;
        NumberOfEnemies = numberOfEnemies;
        DOWN = down;
        RIGHT = right;
        LEFT = left;
        UP = up;
    }
}

public enum WaveDirection
{   
    DOWN = 0,
    RIGHT = 1,
    LEFT = 2,
    UP = 3
}