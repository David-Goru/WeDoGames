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
    Vector3[] spawnerPositions;
    List<Image> signalsImages;

    [SerializeField] int currentWave = 0;
    [SerializeField] float planningTime = 0;
    [SerializeField] Text waveObjectiveText = null;

    [Header("Predetermined enemy waves")]
    [SerializeField] WavesInfo info = null;
    int waveIndex = 0;

    float timer = 0f;
    bool onPlanningPhase = true;
    bool signalsActive = false;
    ObjectPooler objectPooler;
    bool predeterminedWaves;

    void Start()
    {
        predeterminedWaves = info.EnemyWaves.Count > 0;

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
            spawnerPositions[i] = t.position;
            i++;
        }

        foreach(Image img in Signals)
        {
            signalsImages.Add(img);
        }

        objectPooler = ObjectPooler.GetInstance();
        UI.UpdateWaveText(currentWave);
        waveObjectiveText.text = string.Format("Wave objective: {0:0} seconds", 60);
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
                UI.UpdateWaveTimerText(Mathf.RoundToInt(planningTime - timer));
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
                UI.UpdateWaveTimerText(Mathf.RoundToInt(0));             
            }

            if (!signalsActive)
            {
                foreach (EnemyPool enemyPool in info.EnemyWaves[waveIndex].enemyWave)
                {
                    activateSignals(enemyPool);
                }
                signalsActive = true;
            }
        }
        else
        {
            if (enemiesSpawned == 0)
            {
                nextWave();
                signalsActive = false;
            }
            else
            {
                timer += Time.deltaTime;
                UI.UpdateWaveTimerText(Mathf.RoundToInt(timer));
                if (timer < 60) waveObjectiveText.text = string.Format("Wave objective: {0:0} seconds", 60 - timer);
                else waveObjectiveText.text = string.Format("Objective not achieved");
            }
        }   
    }

    void nextWave()
    {
        Master.Instance.UpdateBalance(100); // Wave won
        if (timer <= 60) Master.Instance.UpdateBalance(100); // Objective 1 completed
        if (Nexus.Instance.IsFullHealth) Master.Instance.UpdateBalance(100); // Objective 2 completed
        timer = 0;
        waveObjectiveText.text = string.Format("Wave objective: {0:0} seconds", 60);
        onPlanningPhase = true;
        UI.OpenUpgrades(3);
    }

    void spawnEnemies()
    {
        int spawnerNum = 0;

        for (int i = 0; i < info.EnemyWaves[waveIndex].enemyWave.Count; i++)
        {
            spawnerNum = getSpawnerNum(i);
            for (int j = 0; j < info.EnemyWaves[waveIndex].enemyWave[i].numberOfEnemies; j++)
            {
                Vector3 randomPos = spawnerPositions[spawnerNum] + Vector3.forward * Random.Range(-3f, 3f) + Vector3.right * Random.Range(-3f, 3f);
                objectPooler.SpawnObject(info.EnemyWaves[waveIndex].enemyWave[i].enemy.tag, randomPos, Quaternion.Euler(0, 0, 0));
                enemiesSpawned++;
            }
        }

        foreach (EnemyPool enemyPool in info.EnemyWaves[waveIndex].enemyWave)
        {
            desactivateSignals(enemyPool);
        }

        if (waveIndex < info.EnemyWaves.Count - 1) waveIndex++;
    }

    int getSpawnerNum(int index)
    {
        if (info.EnemyWaves[waveIndex].enemyWave[index].DOWN) return 0;
        else if (info.EnemyWaves[waveIndex].enemyWave[index].RIGHT) return 1;
        else if (info.EnemyWaves[waveIndex].enemyWave[index].LEFT) return 2;
        else return 3; //Default: UP
    }

    void activateSignals(EnemyPool enemyPool)
    {
        if (enemyPool.DOWN) signalActivator(0);
        else if (enemyPool.RIGHT) signalActivator(1);
        else if (enemyPool.LEFT) signalActivator(2);
        else signalActivator(3); //UP
    }

    void signalActivator(int index)
    {
        signalsImages[index].gameObject.SetActive(true);
    }

    void desactivateSignals(EnemyPool enemyPool)
    {
        if (enemyPool.DOWN) signalDesactivator(0);
        else if (enemyPool.RIGHT) signalDesactivator(1);
        else if (enemyPool.LEFT) signalDesactivator(2);
        else signalDesactivator(3); //UP
    }

    void signalDesactivator(int index)
    {
        signalsImages[index].gameObject.SetActive(false);
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
    [Header("Wave enemies")]
    public Pool enemy;
    public int numberOfEnemies;

    [Header("Spawn Positions")]
    public bool DOWN;
    public bool RIGHT;
    public bool LEFT;
    public bool UP;

    public EnemyPool(Pool _enemy, int _numberOfEnemies, bool _up, bool _down, bool _left, bool _right)
    {
        enemy = _enemy;
        numberOfEnemies = _numberOfEnemies;
        DOWN = _down;
        RIGHT = _right;
        LEFT = _left;
        UP = _up;
    }
}