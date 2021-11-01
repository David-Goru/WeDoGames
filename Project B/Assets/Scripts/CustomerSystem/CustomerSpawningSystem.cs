using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawningSystem : MonoBehaviour, IDayNightSwitchable
{
    [SerializeField] float timeBetweenCustomers = 5f;
    [SerializeField] List<Transform> spawnPositions;
    [SerializeField] Pool customerPool;

    float timer = 0f;
    bool isSpawning = false;

    public void OnDayStart()
    {
        startSpawningCustomers();
    }

    public void OnNightStart()
    {
        stopSpawningCustomers();
    }

    void startSpawningCustomers()
    {
        timer = 0f;
        isSpawning = true;
    }

    void stopSpawningCustomers()
    {
        isSpawning = false;
    }

    void Update()
    {
        if (isSpawning && timer >= timeBetweenCustomers) spawnCustomer();
        else timer += Time.deltaTime;
    }

    void spawnCustomer()
    {
        int randomNumber = Random.Range(0, spawnPositions.Count);
        Vector3 position = spawnPositions[randomNumber].position;
        ObjectPooler.GetInstance().SpawnObject(customerPool.tag, position);
        timer = 0f;
    }
}
