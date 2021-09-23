using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Waves : MonoBehaviour
{
    public static int EnemiesRemaining = 0;
    public static bool OnPlanningPhase = true;

    [SerializeField] WavesInfo wavesInfo = null;
    [SerializeField] Objectives objectivesInfo = null;
    [SerializeField] Transform spawners = null;
    [SerializeField] GameObject[] signals = null;
    [SerializeField] Transform objectivesList = null;
    [SerializeField] GameObject objectivePrefab = null;
    [SerializeField] AudioMixerGroup drums = null;
    [SerializeField] GameObject winScreenUI = null;
    [SerializeField] AudioClip waveStartSound = null;
    [SerializeField] AudioClip waveEndSound = null;
    [SerializeField] AudioSource audioSource = null;

    List<Vector3> spawnersPositions; 
    int currentWave = 0;
    int waveIndex = 0;
    float timer = 0f;
    ObjectPooler objectPooler;
    Objective[] gameObjectives;
    bool isSpawning = false;
    float spawnTimer = 0.0f;
    int currentEnemyType = 0;
    int currentEnemyNumber = 0;

    void Start()
    {
        EnemiesRemaining = 0;
        OnPlanningPhase = true;

        spawnersPositions = new List<Vector3>();
        foreach (Transform spawner in spawners) spawnersPositions.Add(spawner.position);

        objectPooler = ObjectPooler.GetInstance();
        UI.UpdateWaveText(currentWave);
        setSignalsVisuals(true);
        setUpObjectives();
    }

    void Update()
    {
        updateWaveState();

        if (isSpawning)
        {
            if (spawnTimer <= 0)
            {
                spawnTimer = 0.25f;
                spawnEnemies();
            }
            spawnTimer -= Time.deltaTime;
        }
    }

    void setUpObjectives()
    {
        int maxIndex = objectivesInfo.LevelObjectives.Length;

        gameObjectives = new Objective[maxIndex > 2 ? 3 : maxIndex];

        if (maxIndex > 0)
        {
            int firstObjective = Random.Range(0, maxIndex);
            gameObjectives[0] = objectivesInfo.LevelObjectives[firstObjective];

            int secondObjective = Random.Range(0, maxIndex);
            if (maxIndex > 1)
            {
                while (firstObjective == secondObjective) secondObjective = Random.Range(0, maxIndex);
                gameObjectives[1] = objectivesInfo.LevelObjectives[secondObjective];
            }

            int thirdObjective = Random.Range(0, maxIndex);
            if (maxIndex > 2)
            {
                while (firstObjective == thirdObjective || secondObjective == thirdObjective) thirdObjective = Random.Range(0, maxIndex);
                gameObjectives[2] = objectivesInfo.LevelObjectives[thirdObjective];
            }
        }

        foreach (Objective objective in gameObjectives)
        {
            objective.UIObject = createObjectiveUI();
            objective.SetDisplayText();
        }
    }

    GameObject createObjectiveUI()
    {
        GameObject uiObject = Instantiate(objectivePrefab);
        uiObject.transform.SetParent(objectivesList, false);

        return uiObject;
    }

    void updateWaveState()
    {
        if (OnPlanningPhase)
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
        OnPlanningPhase = false;
        isSpawning = true;
        spawnTimer = 0.0f;
        setSignalsVisuals(false);
        UI.UpdateWaveText(currentWave);
        UI.ForceCloseUpgrades();
        UI.UpdateWaveTimerText(Mathf.RoundToInt(0));
        startDrums();
        audioSource.clip = waveStartSound;
        audioSource.Play();
        Master.Instance.WavesWithoutBuildingTurrets++;
        Master.Instance.NoActivesUsedInLastWave = true;
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
        if (waveIndex < wavesInfo.EnemyWaves.Count - 1) waveIndex++;
        timer = 0;
        stopDrums();
        audioSource.clip = waveEndSound;
        audioSource.Play();
        if (waveIndex >= 15)
        {
            winScreenUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            OnPlanningPhase = true;
            setSignalsVisuals(true);
            UI.OpenUpgrades(3);
        }
    }

    void spawnEnemies()
    {
        List<int> alreadySpawned = new List<int>();
        int enemiesCounter = 0;

        if (currentEnemyType >= wavesInfo.EnemyWaves[waveIndex].EnemyWave.Count)
        {
            isSpawning = false;
            currentEnemyType = 0;
            currentEnemyNumber = 0;
            return;
        }

        if (currentEnemyNumber >= wavesInfo.EnemyWaves[waveIndex].EnemyWave[currentEnemyType].NumberOfEnemies)
        {
            currentEnemyType++;
            currentEnemyNumber = 0;
        }

        for (int i = currentEnemyType; i < wavesInfo.EnemyWaves[waveIndex].EnemyWave.Count; i++)
        {
            List<int> possibleSpawners = getRandomSpawnerNum(i);
            for (int j = currentEnemyNumber; j < wavesInfo.EnemyWaves[waveIndex].EnemyWave[i].NumberOfEnemies; j++)
            {
                int getRandomSpawn = possibleSpawners[Random.Range(0, possibleSpawners.Count)];

                while (alreadySpawned.Contains(getRandomSpawn))
                {
                    getRandomSpawn = possibleSpawners[Random.Range(0, possibleSpawners.Count)];
                }

                alreadySpawned.Add(getRandomSpawn);

                Vector3 randomPos = spawnersPositions[getRandomSpawn] + Vector3.forward * Random.Range(-3f, 3f) + Vector3.right * Random.Range(-3f, 3f);
                objectPooler.SpawnObject(wavesInfo.EnemyWaves[waveIndex].EnemyWave[i].Enemy.tag, randomPos, Quaternion.Euler(0, 0, 0));
                EnemiesRemaining++;

                if (alreadySpawned.Count == possibleSpawners.Count) alreadySpawned.Clear();
                currentEnemyNumber++;
                enemiesCounter++;

                if (currentEnemyNumber >= wavesInfo.EnemyWaves[waveIndex].EnemyWave[i].NumberOfEnemies)
                {
                    currentEnemyType++;
                    currentEnemyNumber = 0;
                }

                if (enemiesCounter >= 5) return;
            }

            currentEnemyType++;
            if (currentEnemyType >= wavesInfo.EnemyWaves[waveIndex].EnemyWave.Count)
            {
                isSpawning = false;
                currentEnemyType = 0;
                currentEnemyNumber = 0;
            }
        }

        isSpawning = false;
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

    void startDrums()
    {
        StartCoroutine(fadeInDrums());
    }

    IEnumerator fadeInDrums()
    {
        int volume = -80;
        while (volume < 10)
        {
            volume++;
            drums.audioMixer.SetFloat("DrumsVolume", volume);
            yield return new WaitForSeconds(0.01f);
        }
    }

    void stopDrums()
    {
        StartCoroutine(fadeOutDrums());
    }

    IEnumerator fadeOutDrums()
    {
        int volume = 10;
        while (volume > -80)
        {
            volume--;
            drums.audioMixer.SetFloat("DrumsVolume", volume);
            yield return new WaitForSeconds(0.01f);
        }
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