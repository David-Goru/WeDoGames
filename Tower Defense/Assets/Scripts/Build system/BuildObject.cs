﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildObject : MonoBehaviour
{
    [Header("Modifiable values"), Tooltip("Avoid pair values")]
    [SerializeField] int gridSize = 0;

    [Header("References")]
    [SerializeField] Texture buildingGrid = null;
    [SerializeField] Grid grid = null;
    [SerializeField] AudioClip buildSound = null;

    [Header("Debug")]
    [SerializeField] float vertexSize = 0.0f;
    [SerializeField] Texture groundSprite = null;
    [SerializeField] Renderer ground = null;
    [SerializeField] TurretInfo buildingInfo = null;
    [SerializeField] GameObject objectBlueprint = null;
    [SerializeField] Material blueprintMaterial = null;
    [SerializeField] Vector3 lastPos;
    [SerializeField] bool buildable = false;
    [SerializeField] ObjectPooler objectPooler;
    [SerializeField] LayerMask environmentObjectLayer;

    public float VertexSize { get => vertexSize; }

    void Awake()
    {
        if (buildingGrid == null)
        {
            Debug.Log("Building grid not set");
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
    }

    private void Start()
    {
        Master.Instance.BuildObject = this;
        enabled = false;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
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

        if (buildButtonPressed() && !EventSystem.current.IsPointerOverGameObject()) placeObject();
        else if (cancelButtonPressed()) StopBuilding();
    }

    public void StartBuilding(TurretInfo buildingInfo)
    {
        if (!Master.Instance.CheckIfCanAfford(buildingInfo.GetStat(StatType.PRICE))) return;
        Master.Instance.StopAllActions();

        ground.material.SetTexture("_BaseMap", buildingGrid);
        ground.material.SetTextureScale("_BaseMap", new Vector2(gridSize, gridSize));

        this.buildingInfo = buildingInfo;
        enabled = true;
    }

    bool buildButtonPressed()
    {
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
    }

    bool cancelButtonPressed()
    {
        return Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape);
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
                blueprintMaterial.color = Color.red;
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
        if (Physics.CheckSphere(vPos, 0.3f, 1 << LayerMask.NameToLayer("Object")) || Physics.CheckSphere(vPos, 0.3f, 1 << LayerMask.NameToLayer("Enemy")))
        {
            lastPos = pos;
            blueprintMaterial.color = Color.red;
            buildable = false;
        }
        else
        {
            lastPos = vPos;
            blueprintMaterial.color = Color.green;
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
        if (!Master.Instance.UpdateBalance(-buildingInfo.GetStat(StatType.PRICE))) return;
        GameObject turretPlaced = objectPooler.SpawnObject(buildingInfo.GetBuildingPool().tag, objectBlueprint.transform.position, objectBlueprint.transform.rotation);
        grid.SetWalkableNodes(false, objectBlueprint.transform.position, turretPlaced.GetComponent<BuildingRange>().Range, turretPlaced.transform);

        removeEnvirontmentObjects(objectBlueprint.transform.position);
        blueprintMaterial.SetColor("_Color", Color.black);
        objectPooler.ReturnToThePool(objectBlueprint.transform);
        objectBlueprint = null;
        blueprintMaterial = null;
        Master.Instance.RunSound(buildSound);
        Master.Instance.WavesWithoutBuildingTurrets = 0;
        Master.Instance.ActiveTurrets.Add(turretPlaced);
        buildingInfo.NumberOfTurretsPlaced++;

        StartCoroutine("delayCheckPath");

        StopBuilding();
    }

    void removeEnvirontmentObjects(Vector3 position)
    {
        Collider[] objects = Physics.OverlapBox(position, Vector3.one / 2, Quaternion.identity, environmentObjectLayer);
        foreach (Collider col in objects) Destroy(col.gameObject);
    }

    private IEnumerator delayCheckPath()
    {
        yield return new WaitForFixedUpdate();

        List<BaseAI> allEnemies = ActiveEnemies.Instance.enemiesList;

        foreach (BaseAI enemy in allEnemies)
        {
            enemy.checkPath();
        }
    }

    public void StopBuilding()
    {
        if (enabled == false) return;

        if (objectBlueprint != null)
        {
            blueprintMaterial.SetColor("_Color", Color.black);
            objectPooler.ReturnToThePool(objectBlueprint.transform);
            objectBlueprint = null;
            blueprintMaterial = null;
        }

        ground.material.SetTexture("_BaseMap", groundSprite);
        ground.material.SetTextureScale("_BaseMap", new Vector2(1, 1));

        enabled = false;
    }

    public void SetVertexSize()
    {
        vertexSize = 1.0f / gridSize * 10.0f;
    }
}