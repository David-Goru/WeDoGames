using UnityEngine;

/// <summary>
/// Building system. Enabled when selecting an object to build. Starts with the object's blueprint, that the player can move and rotate above the ground. 
/// The object can be moved freely in X and Z axis, but is fixed to Y = 0. The rotation is done from 90 to 90 degrees.
/// Once the player selects a spot (clicking with the blueprint in the mouse position), if there's enough money the object is placed and the building system stopped.
/// The player can start/stop building at any time, but can't start building if there's not enough money for the object selected.
/// </summary>
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
        if (buildingGrid == null)
        {
            Debug.Log("Building grid not set");
            enabled = false;
            return;
        }

        if (stopBuildingButton == null)
        {
            Debug.Log("Stop building button not set");
            enabled = false;
            return;
        }

        ground = GameObject.Find("Ground").GetComponent<Renderer>();
        if (ground == null)
        {
            Debug.Log("Ground not found");
            enabled = false;
            return;
        }
        groundSprite = ground.material.mainTexture;

        SetVertexSize();
        objectPooler = ObjectPooler.GetInstance();
        enabled = false;
    }

    void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (objectBlueprint)
            {
                objectPooler.ReturnToThePool(objectBlueprint.transform);
                objectBlueprint = null;
            }
            return;
        }

        updatePosition();

        if (Input.GetKeyDown(KeyCode.R)) updateRotation();

        if (Input.GetMouseButtonDown(0)) placeObject();
    }

    public void StartBuilding(BuildingInfo buildingInfo)
    {
        if (!MasterHandler.Instance.CheckIfCanAfford(buildingInfo.GetStat(StatType.PRICE))) return;

        stopBuildingButton.SetActive(true);

        ground.material.SetTexture("_MainTex", buildingGrid);
        ground.material.SetTextureScale("_MainTex", new Vector2(gridSize, gridSize));

        this.buildingInfo = buildingInfo;
        enabled = true;
    }

    void updatePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Ground")))
        {
            // Pos = mouse position on world (at Y = 0), vPos = vertex position (on the grid)
            Vector3 pos = new Vector3(hit.point.x, 0, hit.point.z);
            Vector3 vPos = new Vector3(Mathf.Round(hit.point.x / vertexSize) * vertexSize, 0, Mathf.Round(hit.point.z / vertexSize) * vertexSize);

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
        else if (objectBlueprint)
        {
            objectPooler.ReturnToThePool(objectBlueprint.transform);
            objectBlueprint = null;
        }
    }

    void checkPosition(Vector3 pos, Vector3 vPos)
    {
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
        if (objectBlueprint == null) return;

        objectBlueprint.transform.Rotate(0, 90, 0);
    }

    void placeObject()
    {
        if (objectBlueprint == null || buildable == false) return;
        if (!MasterHandler.Instance.UpdateBalance(-buildingInfo.GetStat(StatType.PRICE))) return;

        GameObject turretPlaced = objectPooler.SpawnObject(buildingInfo.GetBuildingPool().tag, objectBlueprint.transform.position, objectBlueprint.transform.rotation);
        grid.SetWalkableNodes(false, objectBlueprint.transform.position, turretPlaced.GetComponent<BuildingRange>().Range);
        
        blueprintMaterial.SetColor("_Color", Color.black);
        objectPooler.ReturnToThePool(objectBlueprint.transform);
        objectBlueprint = null;
        blueprintMaterial = null;

        StopBuilding();
    }

    public void StopBuilding()
    {
        stopBuildingButton.SetActive(false);

        if (objectBlueprint != null)
        {
            blueprintMaterial.SetColor("_Color", Color.black);
            objectPooler.ReturnToThePool(objectBlueprint.transform);
            objectBlueprint = null;
            blueprintMaterial = null;
        }

        ground.material.SetTexture("_MainTex", groundSprite);
        ground.material.SetTextureScale("_MainTex", new Vector2(1, 1));

        enabled = false;
    }

    public void SetVertexSize()
    {
        vertexSize = 1.0f / gridSize * 10.0f;
    }
}