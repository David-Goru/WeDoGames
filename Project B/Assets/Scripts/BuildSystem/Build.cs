using UnityEngine;

public class Build : MonoBehaviour, IDayNightSwitchable
{
    [SerializeField] private GameObject buildUIPrefab;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private KeyCode rotationKey;
    [SerializeField] private LayerMask buildableFloor;
    [SerializeField] private LayerMask buildableFurniture;

    private GameObject ui;
    private Furniture furnitureSelected;
    private bool isMoving;
    private Vector3 initialPosition;
    private Vector3 currentPosition;
    private int initialYRotation;
    private int currentYRotation;
    private Vector3 lastMousePosition;
    private bool canBePlaced;

    private void Awake()
    {
        ui = Instantiate(buildUIPrefab, GameObject.Find("UI").transform);
        ui.SetActive(false);
    }

    public void StartBuilding(Furniture furniture)
    {
        if (enabled) Stop();
        
        isMoving = false;
        furnitureSelected = Instantiate(furniture);
        start();
    }

    public void StartMoving(Furniture furniture)
    {
        if (enabled) Stop();
        
        isMoving = true;
        furnitureSelected = furniture;
        
        var furnitureTransform = furniture.transform;
        initialPosition = furnitureTransform.position;
        initialYRotation = Mathf.RoundToInt(furnitureTransform.eulerAngles.y);
        
        start();
    }

    private void start()
    {
        lastMousePosition = Vector3.negativeInfinity;
        furnitureSelected.transform.localScale = Vector3.one * 1.1f;
        enabled = true;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) tryToPlace();
        else if (lastMousePosition != Input.mousePosition) updatePosition();
        else if (Input.GetKeyDown(rotationKey)) rotate();
    }

    private void updatePosition()
    {
        if (!furnitureSelected) return;

        lastMousePosition = Input.mousePosition;
        currentPosition = getFurniturePosition();
        furnitureSelected.transform.position = currentPosition;
        checkIfCanBePlaced();
        updateVisuals();
    }

    private void rotate()
    {
        if (!furnitureSelected) return;

        currentYRotation += 90;
        furnitureSelected.transform.eulerAngles = new Vector3(0, currentYRotation, 0);
    }

    public void Stop()
    {
        if (isMoving) repositionFurniture();
        
        enabled = false;
        furnitureSelected = null;
    }

    private void repositionFurniture()
    {
        if (!furnitureSelected) return;

        var furnitureTransform = furnitureSelected.transform;
        furnitureTransform.position = initialPosition;
        furnitureTransform.eulerAngles = new Vector3(0, initialYRotation, 0);
        furnitureTransform.localScale = Vector3.one;
    }

    private void tryToPlace()
    {
        if (canBePlaced) place();
    }

    private void checkIfCanBePlaced()
    {
        Collider[] hitColliders = new Collider[2];
        int numberOfHitCollidersFound = Physics.OverlapSphereNonAlloc(currentPosition + Vector3.up * 0.01f, 0.005f, hitColliders);
        canBePlaced = numberOfHitCollidersFound <= 1; // 1 = the object in hand
    }

    private Vector3 getFurniturePosition()
    {
        if (!mainCamera)
        {
            Debug.LogWarning("Main camera not found on Build component");
            return new Vector3(0, 100, 0);
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (furnitureSelected.Data.BuildableAboveFurniture && Physics.Raycast(ray, out var hit, 100, buildableFurniture)) return positionAboveParent(hit);
        if (furnitureSelected.Data.BuildableAboveFloor && Physics.Raycast(ray, out hit, 100, buildableFloor)) return roundedPosition(hit.point);
        
        return new Vector3(0, 100, 0);
    }

    private static Vector3 roundedPosition(Vector3 rawPosition, float divisions = 1.0f)
    {
        float x = Mathf.RoundToInt(rawPosition.x * divisions) / divisions;
        float z = Mathf.RoundToInt(rawPosition.z * divisions) / divisions;

        return new Vector3(x, rawPosition.y, z);
    }

    private static Vector3 positionAboveParent(RaycastHit hit)
    {
        Vector3 position = hit.transform.parent.position;
        position.y = hit.point.y;

        return position;
    }

    private void updateVisuals()
    {
        Color newColor = canBePlaced ? Color.green : Color.red;

        Transform model = furnitureSelected.transform.Find("Model");
        model.GetComponent<Renderer>().material.color = newColor;
    }

    private void place()
    {
        Transform furnitureTransform = furnitureSelected.transform;
        furnitureTransform.localScale = Vector3.one;
        
        Transform model = furnitureTransform.Find("Model");
        model.GetComponent<Renderer>().material.color = Color.white;
        enabled = false;
    }

    public void OnDayStart()
    {
        ui.SetActive(false);
    }

    public void OnNightStart()
    {
        ui.SetActive(true);
    }
}