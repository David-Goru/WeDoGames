using System;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private int numberOfSpots;
    [SerializeField] private int counterSize;
    [SerializeField] private GameObject counterPrefab;
    [SerializeField] private GameObject counterSpotPrefab;

    private GameObject counterModel;

    private void Start()
    {
        initializeModel();
        initializeSpots();
    }

    private void initializeModel()
    {
        counterModel = Instantiate(counterPrefab);
    }

    private void initializeSpots()
    {
        for (int i = 0; i < numberOfSpots; i++)
        {
            GameObject counterSpot = Instantiate(counterSpotPrefab, counterModel.transform);
            Vector3 offset = new Vector3(-counterSize / 2.0f + (float)counterSize / numberOfSpots / 2.0f + (float)counterSize / numberOfSpots * i, 0, 1);
            counterSpot.transform.position += offset;
        }
    }
}