using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Build : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private KeyCode rotationKey;
    [SerializeField] private LayerMask buildableFloor;
    [SerializeField] private LayerMask buildableFurniture;
    
    private Furniture furnitureSelected;
    private bool isMoving;
    private Vector3 initialPosition;
    private Vector3 currentPosition;
    private int initialYRotation;
    private int currentYRotation;
    private Vector3 lastMousePosition;

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
    }

    private void tryToPlace()
    {
        if (checkIfCanPlace()) place();
    }

    private bool checkIfCanPlace()
    {
        return true;
    }

    private Vector3 getFurniturePosition()
    {
        if (!mainCamera)
        {
            Debug.LogWarning("Main camera not found on Build component");
            return new Vector3(0, 100, 0);
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (furnitureSelected.Data.BuildableAboveFurniture && Physics.Raycast(ray, out var hit, 100, buildableFurniture)) return hit.point;
        if (furnitureSelected.Data.BuildableAboveFloor && Physics.Raycast(ray, out hit, 100, buildableFloor)) return hit.point;
        
        return new Vector3(0, 100, 0);
    }

    private void place()
    {
        enabled = false;
    }
}