using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesHandler : MonoBehaviour
{
    public static int EnemiesSpawned = 0;

    public const int ENEMIES_PER_WAVE_MULTIPLIER = 4;

    [SerializeField] UpgradesUI upgradesUI = null;

    public MasterInfo MasterInfo;
    public Transform Spawners;
    public Text WaveTimerText;
    Vector3[] spawnerPositions;

    [SerializeField] int currentWave = 0;
    [SerializeField] float planningTime = 0;

    float timer = 0f;
    bool onPlanningPhase = true;
    ObjectPooler objectPooler;

    /// <summary>
    /// Initialize waves info
    /// </summary>
    void Start()
    {
        // Initialize spawn positions
        spawnerPositions = new Vector3[Spawners.childCount];
        int i = 0;
        foreach (Transform t in Spawners)
        {
            spawnerPositions[i] = t.position;
            i++;
        }

        // Get object pooler
        objectPooler = ObjectPooler.GetInstance();
    }

    /// <summary>
    /// Update the waves
    /// </summary>
    void Update()
    {
        updateWaveState();
    }

    /// <summary>
    /// Update state of the wave, depending on whether it is on planning phase or on enemies attacking phase
    /// </summary>
    void updateWaveState()
    {
        if (onPlanningPhase)
        {
            if (timer < planningTime)
            {
                // Update UI text and timer
                WaveTimerText.text = string.Format("Next wave: {0:0} seconds", planningTime - timer);
                timer += Time.deltaTime;
            }
            else
            {
                // Create new wave
                currentWave++;
                timer = 0;
                onPlanningPhase = false;
                spawnEnemies();
                upgradesUI.CloseUpgrades();
            }
        }
        else
        {
            if (EnemiesSpawned == 0) nextWave();
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
        timer = 0;
        onPlanningPhase = true;
        upgradesUI.EnableRandomUpgrades(2);
    }

    /// <summary>
    /// Spawn enemies at random positions (from the spawnerPositions list)
    /// </summary>
    void spawnEnemies()
    {
        for (int i = 0; i < currentWave * ENEMIES_PER_WAVE_MULTIPLIER; i++)
        {
            Vector3 randomPos = spawnerPositions[Random.Range(0, spawnerPositions.Length)] + Vector3.forward * Random.Range(-3, 3) + Vector3.right * Random.Range(-3, 3);
            objectPooler.SpawnObject(getRandomEnemy(), randomPos, Quaternion.Euler(0, 0, 0));
        }
    }

    string getRandomEnemy()
    {
        int maxEnemyId = currentWave > MasterInfo.GetEnemiesSet().Length ? MasterInfo.GetEnemiesSet().Length : currentWave;
        int enemyId = Random.Range(0, maxEnemyId);
        return MasterInfo.GetEnemiesSet()[enemyId].tag;
    }

    public static void EnemyKilled()
    {
        EnemiesSpawned--;
    }
}