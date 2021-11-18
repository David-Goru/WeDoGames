using UnityEngine;

public class ObjectDetector : MonoBehaviour, ILoadable
{
    private readonly Vector3 offset = new Vector3(0f, 0.5f, 0f);

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float interactRange;

    private bool isCreated; //Temporal

    private void Update()
    {
        if(isCreated) checkForObjects();
    }

    private void checkForObjects()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + offset, transform.forward, out hit, interactRange, layerMask)) //Can I interact with this object?
        {
            Debug.Log("Facing an interactable object");
        }
    }

    public void Create()
    {
        isCreated = true;
    }
}