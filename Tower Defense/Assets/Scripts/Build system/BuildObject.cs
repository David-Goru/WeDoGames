using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour
{
    [Header("Modifiable values"), Tooltip("Avoid pair values")]
    [SerializeField] int gridSize = 0;

    [Header("References")]
    [SerializeField] Texture buildingGrid = null;
    [SerializeField] Grid grid = null;
    [SerializeField] GameObject stopBuildingButton = null;

    [Header("Debug")]
    [SerializeField] float vertexSize = 0.0f;
    [SerializeField] Texture groundSprite = null;
    [SerializeField] Renderer ground = null;
    [SerializeField] BuildingInfo buildingInfo = null;
    [SerializeField] GameObject objectBlueprint = null;
    [SerializeField] Material blueprintMaterial = null;
    [SerializeField] Vector3 lastPos;
    [SerializeField] bool buildable = false;
    [SerializeField] ObjectPooler objectPooler;

    public float VertexSize { get => vertexSize; }

    void Awake()
    {
        // Building grid
        if (buildingGrid == null)
        {
            Debug.Log("Building grid not set");
            enabled = false;
            return;
        }

        // Stop building button
        if (stopBuildingButton == null)
        {
            Debug.Log("Stop building button not set");
            enabled = false;
            return;
        }

        // Ground
        ground = GameObject.Find("Ground").GetComponent<Renderer>();
        if (ground == null)
        {
            Debug.Log("Ground not found");
            enabled = false;
            return;
        }
        groundSprite = ground.material.mainTexture;

        // Others
        SetVertexSize();
        objectPooler = ObjectPooler.GetInstance();
        enabled = false;
    }

    void Update()
    {
        // If mouse over UI, stop building
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (objectBlueprint)
            {
                objectPooler.ReturnToThePool(objectBlueprint.transform);
                objectBlueprint = null;
            }
            return;
        }

        // Check position
        updatePosition();

        // Change rotation
        if (Input.GetKeyDown(KeyCode.R)) updateRotation();

        // Place object
        if (Input.GetMouseButtonDown(0)) placeObject();
    }

    public void StartBuilding(BuildingInfo buildingInfo)
    {
        stopBuildingButton.SetActive(true);

        // Check if the player can affor the building
        if (!MasterHandler.Instance.CheckIfCanAfford(buildingInfo.GetStat(StatType.PRICE))) return;

        // Set ground to building mode
        ground.material.SetTexture("_MainTex", buildingGrid);
        ground.material.SetTextureScale("_MainTex", new Vector2(gridSize, gridSize));

        this.buildingInfo = buildingInfo;
        enabled = true;
    }

    void updatePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if mouse is over background
        if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Ground")))
        {
            // Pos = mouse position (at Y = 0), vPos = vertex position (on the grid)
            Vector3 pos = new Vector3(hit.point.x, 0, hit.point.z);
            Vector3 vPos = new Vector3(Mathf.Round(hit.point.x / vertexSize) * vertexSize, 0, Mathf.Round(hit.point.z / vertexSize) * vertexSize);

            // If object blueprint is not already on the map, build it
            if (objectBlueprint == null)
            {
                objectBlueprint = objectPooler.SpawnObject(buildingInfo.GetBuildingBlueprintPool().tag, pos, Quaternion.Euler(0, 0, 0));
                lastPos = pos;
                blueprintMaterial = objectBlueprint.transform.Find("Model").GetComponent<Renderer>().material;
                blueprintMaterial.SetColor("_Color", Color.red);
                checkPosition(pos, vPos);
            }
            else if (lastPos != pos && lastPos != vPos) checkPosition(pos, vPos);
        }
        else if (objectBlueprint) // Mouse out of the map, delete object blueprint
        {
            objectPooler.ReturnToThePool(objectBlueprint.transform);
            objectBlueprint = null;
        }
    }

    void checkPosition(Vector3 pos, Vector3 vPos)
    {
        // Check if there are objects on the vertex position
        if (Physics.CheckSphere(vPos, 0.3f, 1 << LayerMask.NameToLayer("Object")))
        {
            lastPos = pos;
            blueprintMaterial.SetColor("_Color", Color.red);
            buildable = false;
        }
        else
        {
            lastPos = vPos;
            blueprintMaterial.SetColor("_Color", Color.green);
            buildable = true;
        }
        objectBlueprint.transform.position = lastPos;
    }

    void updateRotation()
    {
        // If object blueprint is not on the map, don't rotate
        if (objectBlueprint == null) return;

        objectBlueprint.transform.Rotate(0, 90, 0);
    }

    void placeObject()
    {
        // Pay for the building
        if(!MasterHandler.Instance.UpdateBalance(-buildingInfo.GetStat(StatType.PRICE)))
        {
            // The player can't afford it, so don't do anything
            return;
        }

        // If object blueprint is not on the map or it can't be built, do nothing
        if (objectBlueprint == null || buildable == false) return;

        // Activate turret
        GameObject turretPlaced = objectPooler.SpawnObject(buildingInfo.GetBuildingPool().tag, objectBlueprint.transform.position, objectBlueprint.transform.rotation);
        grid.SetWalkableNodes(false, objectBlueprint.transform.position, turretPlaced.GetComponent<BuildingRange>().Range);
        

        // Get rid of blueprint
        blueprintMaterial.SetColor("_Color", Color.black);
        objectPooler.ReturnToThePool(objectBlueprint.transform);
        objectBlueprint = null;
        blueprintMaterial = null;

        StopBuilding();
    }

    public void StopBuilding()
    {
        stopBuildingButton.SetActive(false);

        // Get rid of blueprint
        if (objectBlueprint != null)
        {
            blueprintMaterial.SetColor("_Color", Color.black);
            objectPooler.ReturnToThePool(objectBlueprint.transform);
            objectBlueprint = null;
            blueprintMaterial = null;
        }

        // Reset ground texture
        ground.material.SetTexture("_MainTex", groundSprite);
        ground.material.SetTextureScale("_MainTex", new Vector2(1, 1));

        enabled = false;
    }

    public void SetVertexSize()
    {
        vertexSize = 1.0f / gridSize * 10.0f;
    }
}