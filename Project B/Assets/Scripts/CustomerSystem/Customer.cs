using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour, IPooledObject
{
    CustomerWaitingPlace[] customerWaitingPlaces;
    NavMeshAgent agent;
    bool isInWaitingPlace = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        customerWaitingPlaces = FindObjectsOfType<CustomerWaitingPlace>();
    }

    private void tryToGoToAWaitingPlace()
    {
        foreach (var place in customerWaitingPlaces)
        {
            if (!place.IsOccupied)
            {
                goToWaitingPlace(place);
                return;
            }
        }
    }

    private void Update()
    {
        if (!isInWaitingPlace) tryToGoToAWaitingPlace();
    }

    void goToWaitingPlace(CustomerWaitingPlace place)
    {
        place.IsOccupied = true;
        isInWaitingPlace = true;
        agent.SetDestination(place.transform.position);
    }

    public void OnObjectSpawn()
    {
        tryToGoToAWaitingPlace();
    }
}