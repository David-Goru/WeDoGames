using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the waves, enemies spawned on every wave and points earned.
/// </summary>
public class WavesHandler : MonoBehaviour
{
    static int enemiesSpawned = 0;

    public const int ENEMIES_PER_WAVE_MULTIPLIER = 5;

    public MasterInfo MasterInfo;
    public Transform Spawners;
    public Image[] Signals;
    public Text WaveTimerText;
    Vector3[] spawnerPositions;
    List<Image> signalsImages;

    [SerializeField] int currentWave = 0;
    [SerializeField] float planningTime = 0;

    [Header("Predetermined enemy waves")]
    public List<EnemyList> enemyWaves;
    private int waveIndex = 0;

    float timer = 0f;
    bool onPlanningPhase = true;
    ObjectPooler objectPooler;
    bool predeterminedWaves;

    void Start()
    {
        predeterminedWaves = enemyWaves.Count > 0;

        int childsActive = 0;
        for (int j = 0; j < Spawners.childCount; j++)
        {
            if (Spawners.GetChild(j).gameObject.activeSelf) childsActive++;
        }

        spawnerPositions = new Vector3[childsActive];
        signalsImages = new List<Image>();

        int i = 0;
        foreach (Transform t in Spawners)
        {
            if (t.gameObject.activeSelf)
            {
                addImageSpawner(t);

                spawnerPositions[i] = t.position;
                i++;
            }
        }

        objectPooler = ObjectPooler.GetInstance();
        UI.UpdateWaveText(currentWave);
    }

    void Update()
    {
        updateWaveState();
    }

    void updateWaveState()
    {
        if (onPlanningPhase)
        {
            if (timer < planningTime)
            {
                WaveTimerText.text = string.Format("Next wave: {0:0} seconds", planningTime - timer);
                timer += Time.deltaTime;
            }
            else
            {
                currentWave++;
                UI.UpdateWaveText(currentWave);
                timer = 0;
                onPlanningPhase = false;
                spawnEnemies();
                UI.CloseUpgrades();
            }
        }
        else
        {
            if (enemiesSpawned == 0) nextWave();
            else
            {
                timer += Time.deltaTime;
                if (timer < 60) WaveTimerText.text = string.Format("Wave objective: {0:0} seconds", 60 - timer);
                else WaveTimerText.text = string.Format("Objective not achieved");
            }
        }
    }

    void nextWave()
    {
        Master.Instance.UpdateBalance(100); // Wave won
        if (timer <= 60) Master.Instance.UpdateBalance(100); // Objective 1 completed
        if (Nexus.Instance.IsFullHealth) Master.Instance.UpdateBalance(100); // Objective 2 completed
        timer = 0;
        onPlanningPhase = true;
        UI.OpenUpgrades(3);
    }

    void spawnEnemies()
    {
        int spawnerNum = getSpawnerNum();
        if (!predeterminedWaves) //Random waves
        {
            for (int i = 0; i < currentWave * ENEMIES_PER_WAVE_MULTIPLIER; i++)
            {
                int randInd = Random.Range(0, spawnerNum);
                Vector3 randomPos = spawnerPositions[randInd] + Vector3.forward * Random.Range(-3, 3) + Vector3.right * Random.Range(-3, 3);
                objectPooler.SpawnObject(getRandomEnemy(), randomPos, Quaternion.Euler(0, 0, 0));
                StartCoroutine(activateSignal(randInd));
                enemiesSpawned++;
            }
        }
        else //Predetermined waves
        {
            for (int i = 0; i < enemyWaves[waveIndex].enemyWave.Count; i++)
            {
                for(int j = 0; j < enemyWaves[waveIndex].enemyWave[i].numberOfEnemies; j++)
                {
                    int randInd = Random.Range(0, spawnerNum);
                    Vector3 randomPos = spawnerPositions[randInd] + Vector3.forward * Random.Range(-3, 3) + Vector3.right * Random.Range(-3, 3);
                    objectPooler.SpawnObject(enemyWaves[waveIndex].enemyWave[i].enemy.tag, randomPos, Quaternion.Euler(0, 0, 0));
                    StartCoroutine(activateSignal(randInd));
                    enemiesSpawned++;
                }
            }

            if(waveIndex < enemyWaves.Count - 1) waveIndex++;
        }
    }

    private int getSpawnerNum()
    {
        int spawnersNum = 1; //Round 1 - 4
        if (currentWave >= 5 && currentWave <= 9 && spawnerPositions.Length > 1 || (currentWave >= 5 && spawnerPositions.Length == 2)) //Round 5 - 9
            spawnersNum = 2;
        else if (currentWave >= 10 && currentWave <= 14 && spawnerPositions.Length > 2 || (currentWave >= 10 && spawnerPositions.Length == 3)) //Round 10 - 14
            spawnersNum = 3;
        else if (currentWave >= 15 && spawnerPositions.Length > 3) //Round 15++
            spawnersNum = 4;
        return spawnersNum;
    }

    private void addImageSpawner(Transform t)
    {
        if (t.gameObject.name == "SpawnerAbajo") signalsImages.Add(Signals[0]);
        else if (t.gameObject.name == "SpawnerDerecha") signalsImages.Add(Signals[1]);
        else if (t.gameObject.name == "SpawnerIzquierda") signalsImages.Add(Signals[2]);
        else signalsImages.Add(Signals[3]);
    }

    private IEnumerator activateSignal(int index)
    {
        signalsImages[index].gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        signalsImages[index].gameObject.SetActive(false);
    }

    string getRandomEnemy()
    {
        int maxEnemyId = currentWave > MasterInfo.GetEnemiesSet().Length ? MasterInfo.GetEnemiesSet().Length : currentWave;
        int enemyId = Random.Range(0, maxEnemyId);
        return MasterInfo.GetEnemiesSet()[enemyId].tag;
    }

    public static void EnemyKilled()
    {
        enemiesSpawned--;
    }
}

[System.Serializable]
public class EnemyList
{
    public List<EnemyPool> enemyWave;

    public EnemyList(List<EnemyPool> _enemyWave)
    {
        enemyWave = _enemyWave;
    }
}

[System.Serializable]
public class EnemyPool
{
    public Pool enemy;
    public int numberOfEnemies;

    public EnemyPool(Pool _enemy, int _numberOfEnemies)
    {
        enemy = _enemy;
        numberOfEnemies = _numberOfEnemies;
    }
}