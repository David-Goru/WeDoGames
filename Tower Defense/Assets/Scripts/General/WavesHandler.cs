using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesHandler : MonoBehaviour
{
    public const int ENEMIES_PER_WAVE_MULTIPLIER = 100;

    [SerializeField] UpgradesUI upgradesUI;

    public MasterInfo MasterInfo;
    public Transform Spawners;
    public Text WaveTimerText;
    Vector3[] spawnerPositions;

    [SerializeField] int currentWave = 0;
    [SerializeField] float nextWaveTimer = 0;
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
            if (timer < nextWaveTimer)
            {
                // Update UI text and timer
                WaveTimerText.text = string.Format("End of wave: {0:0} seconds", nextWaveTimer - timer);
                timer += Time.deltaTime;
            }
            else
            {
                // Set planning time
                timer = 0;
                onPlanningPhase = true;
                upgradesUI.EnableRandomUpgrades(2);
            }
        }
    }

    /// <summary>
    /// Spawn enemies at random positions (from the spawnerPositions list)
    /// </summary>
    void spawnEnemies()
    {
        for (int i = 0; i < currentWave * ENEMIES_PER_WAVE_MULTIPLIER; i++)
        {
            objectPooler.SpawnObject(MasterInfo.GetEnemiesSet()[0].tag, spawnerPositions[Random.Range(0, spawnerPositions.Length)], Quaternion.Euler(0, 0, 0));
        }
    }
}