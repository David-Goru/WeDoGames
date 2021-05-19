using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the waves, enemies spawned on every wave and points earned.
/// </summary>
public class WavesHandler : MonoBehaviour
{
    static int enemiesSpawned = 0;

    public const int ENEMIES_PER_WAVE_MULTIPLIER = 5;

    [SerializeField] UpgradesUI upgradesUI = null;

    public MasterInfo MasterInfo;
    public Transform Spawners;
    public Image[] Signals;
    public Text WaveTimerText;
    Vector3[] spawnerPositions;

    [SerializeField] int currentWave = 0;
    [SerializeField] float planningTime = 0;

    float timer = 0f;
    bool onPlanningPhase = true;
    ObjectPooler objectPooler;

    void Start()
    {
        spawnerPositions = new Vector3[Spawners.childCount];
        int i = 0;
        foreach (Transform t in Spawners)
        {
            spawnerPositions[i] = t.position;
            i++;
        }

        objectPooler = ObjectPooler.GetInstance();
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
                timer = 0;
                onPlanningPhase = false;
                spawnEnemies();
                upgradesUI.CloseUpgrades();
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
        MasterHandler.Instance.UpdatePoints(1); // Wave won
        if (timer <= 60) MasterHandler.Instance.UpdatePoints(1); // Objective 1 completed
        if (Nexus.Instance.IsFullHealth) MasterHandler.Instance.UpdatePoints(1); // Objective 2 completed
        timer = 0;
        onPlanningPhase = true;
        upgradesUI.EnableRandomUpgrades(2);
    }

    void spawnEnemies()
    {
        int spawnerNum = getSpawnerNum();
        for (int i = 0; i < currentWave * ENEMIES_PER_WAVE_MULTIPLIER; i++)
        {
            int randInd = Random.Range(0, spawnerNum);
            Vector3 randomPos = spawnerPositions[randInd] + Vector3.forward * Random.Range(-3, 3) + Vector3.right * Random.Range(-3, 3);
            objectPooler.SpawnObject(getRandomEnemy(), randomPos, Quaternion.Euler(0, 0, 0));
            StartCoroutine(activateSignal(randInd));
            enemiesSpawned++;
        }
    }

    private int getSpawnerNum()
    {
        int spawnersNum = 3; //Round 1 - 4
        if (currentWave >= 5 && currentWave <= 9) //Round 5 - 9
            spawnersNum = 2;
        else if (currentWave >= 10 && currentWave <= 14) //Round 10 - 14
            spawnersNum = 3;
        else if (currentWave >= 15) //Round 15++
            spawnersNum = 4;
        return spawnersNum;
    }

    private IEnumerator activateSignal(int index)
    {
        Signals[index].gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        Signals[index].gameObject.SetActive(false);
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