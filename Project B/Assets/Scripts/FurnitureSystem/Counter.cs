using UnityEngine;

public class Counter : MonoBehaviour
{
    private CustomerWaitingPlace[] spots;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        spots = FindObjectsOfType<CustomerWaitingPlace>();
    }
}