using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesHandler : MonoBehaviour
{
    public Transform Spawners;
    public Text WaveTimerText;
    Vector3[] spawnerPositions;

    [SerializeField] int currentWave;
    [SerializeField] float nextWaveTimer;

    float timer = 0f;

    void Start()
    {
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
        if (timer < nextWaveTimer)
        {
            WaveTimerText.text = string.Format("Next wave: {0:0} seconds", nextWaveTimer - timer);
            timer += Time.deltaTime;
        }
        else
        {
            currentWave++;
            timer = 0;
            spawnEnemies();
        }
    }

    void spawnEnemies()
    {
        for (int i = 0; i < currentWave * 4; i++)
        {
            Debug.Log("Enemy spawned at " + spawnerPositions[Random.Range(0, spawnerPositions.Length)]);            
        }
    }
}