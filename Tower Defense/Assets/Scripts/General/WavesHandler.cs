using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesHandler : MonoBehaviour
{
    public const int ENEMIES_PER_WAVE_MULTIPLIER = 4;

    public Transform Spawners;
    public Text WaveTimerText;
    Vector3[] spawnerPositions;

    [SerializeField] int currentWave = 0;
    [SerializeField] float nextWaveTimer = 0;

    float timer = 0f;

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
    }

    void Update()
    {
        updateWaveState();
    }

    void updateWaveState()
    {
        if (timer < nextWaveTimer)
        {
            // Update UI text and timer
            WaveTimerText.text = string.Format("Next wave: {0:0} seconds", nextWaveTimer - timer);
            timer += Time.deltaTime;
        }
        else
        {
            // Create new wave
            currentWave++;
            timer = 0;
            spawnEnemies();
        }
    }

    // Spawn enemies at a random spawner
    void spawnEnemies()
    {
        for (int i = 0; i < currentWave * ENEMIES_PER_WAVE_MULTIPLIER; i++)
        {
            // TEST
            Debug.Log("Enemy spawned at " + spawnerPositions[Random.Range(0, spawnerPositions.Length)]);            
        }
    }
}