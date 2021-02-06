using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour
{
    [Header("Grid info")]
    public int GridSize;
    public float VertexSize;

    [Header("Other")]
    public Texture BuildingGrid;
    Texture groundSprite;
    Renderer ground;

    // Object info
    BuildingInfo buildingInfo;
    GameObject objectBlueprint;
    Vector3 lastPos;
    bool buildable;

    ObjectPooler objectPooler;

    void Start()
    {
        if (BuildingGrid == null)
        {
            Debug.Log("Building grid not set");
            this.enabled = false;
            return;
        }

        ground = GameObject.Find("Ground").GetComponent<Renderer>();
        if (ground == null)
        {
            Debug.Log("Ground not found");
            this.enabled = false;
            return;
        }
        groundSprite = ground.material.mainTexture;

        objectPooler = ObjectPooler.GetInstance();

        this.enabled = false;
    }

    void Update()
    {
        // Check position
        updatePosition();

        // Change rotation
        if (Input.GetKeyDown(KeyCode.R)) updateRotation();

        // Place object
        if (Input.GetMouseButtonDown(0)) placeObject();
    }

    public void StartBuilding(BuildingInfo buildingInfo)
    {
        // Check if the player can affor the building
        //if (!MasterHandler.CheckIfCanAfford(buildingInfo.GetStat())) return;

        // Set ground to building mode
        ground.material.SetTexture("_MainTex", BuildingGrid);
        ground.material.SetTextureScale("_MainTex", new Vector2(GridSize, GridSize));

        this.buildingInfo = buildingInfo;
        this.enabled = true;
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
            Vector3 vPos = new Vector3(Mathf.Round(hit.point.x / VertexSize) * VertexSize, 0, Mathf.Round(hit.point.z / VertexSize) * VertexSize);

            // If object blueprint is not already on the map, build it
            if (objectBlueprint == null)
            {
                objectBlueprint = objectPooler.SpawnObject(buildingInfo.GetBuildingBlueprintPool().tag, pos, Quaternion.Euler(0, 0, 0));
                lastPos = pos;
                objectBlueprint.transform.Find("Model").GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                checkPosition(pos, vPos);
            }
            else if (lastPos != pos && lastPos != vPos) checkPosition(pos, vPos);
        }
        else if (objectBlueprint)
        {
            objectPooler.ReturnToThePool(objectBlueprint.transform);
            objectBlueprint = null; // Mouse out of the map, delete object blueprint
        }
    }

    void checkPosition(Vector3 pos, Vector3 vPos)
    {
        // Check if there are objects on the vertex position
        if (Physics.CheckSphere(vPos, 0.3f, 1 << LayerMask.NameToLayer("Object")))
        {
            lastPos = pos;
            objectBlueprint.transform.Find("Model").GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            buildable = false;
        }
        else
        {
            lastPos = vPos;
            objectBlueprint.transform.Find("Model").GetComponent<Renderer>().material.SetColor("_Color", Color.green);
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
        /*if(!MasterHandler.UpdateBalance(-buildingInfo.GetPrice()))
        {
            // The player can't afford it, so don't do anything
            return;
        }*/

        // If object blueprint is not on the map or it can't be built, do nothing
        if (objectBlueprint == null || buildable == false) return;

        // Activate turret
        objectPooler.SpawnObject(buildingInfo.GetBuildingPool().tag, objectBlueprint.transform.position, objectBlueprint.transform.rotation);

        // Get rid of blueprint
        objectBlueprint.transform.Find("Model").GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        objectPooler.ReturnToThePool(objectBlueprint.transform);
        objectBlueprint = null;

        StopBuilding();
    }

    public void StopBuilding()
    {
        // Get rid of blueprint
        if (objectBlueprint != null)
        {
            objectBlueprint.transform.Find("Model").GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            objectPooler.ReturnToThePool(objectBlueprint.transform);
        }

        // Reset ground texture
        ground.material.SetTexture("_MainTex", groundSprite);
        ground.material.SetTextureScale("_MainTex", new Vector2(1, 1));

        this.enabled = false;
    }
}