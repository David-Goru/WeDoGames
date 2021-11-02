using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour, IPooledObject
{
    [SerializeField] string[] productsToAsk;
    [SerializeField] float waitingTime = 15f;
    CustomerWaitingPlace[] customerWaitingPlaces;
    CustomerWaitingPlace currentCustomerWaitingPlace;
    NavMeshAgent agent;
    bool isGoingIntoAWaitingPlace = false;
    bool isInWaitingPlace = false;

    float timer = 0f;

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
        if (!isGoingIntoAWaitingPlace && !isInWaitingPlace) tryToGoToAWaitingPlace();
        else if(isGoingIntoAWaitingPlace)
        {
            if(agent.remainingDistance <= 0.1f)
            {
                isInWaitingPlace = true;
                isGoingIntoAWaitingPlace = false;
                askForSomething();
            }
        }
        else
        {
            if (timer >= waitingTime)
            {
                ObjectPooler.GetInstance().ReturnToThePool(transform);
                currentCustomerWaitingPlace.IsOccupied = false;
            }
            else timer += Time.deltaTime;
        }
    }

    private void askForSomething()
    {
        int randomNumber = Random.Range(0, productsToAsk.Length);
        print("Please give me a " + productsToAsk[randomNumber]);
    }

    void goToWaitingPlace(CustomerWaitingPlace place)
    {
        place.IsOccupied = true;
        currentCustomerWaitingPlace = place;
        isGoingIntoAWaitingPlace = true;
        agent.SetDestination(place.transform.position);
    }

    public void OnObjectSpawn()
    {
        timer = 0f;
        isGoingIntoAWaitingPlace = false;
        isInWaitingPlace = false;
        tryToGoToAWaitingPlace();
    }
}