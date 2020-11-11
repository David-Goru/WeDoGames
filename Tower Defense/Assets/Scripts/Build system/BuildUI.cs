using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    public GameObject ObjectUIPrefab;
    public Transform BuildingsListUI;
    public MasterInfo MasterInfo;
    public GameObject Master;

    Transform objectUI;

    void Start()
    {
        BuildingInfo buildingInfo;

        // Load all buidings
        for (int i = 0; i < MasterInfo.GetBuildingsSet().Length; i++)
        {
            buildingInfo = MasterInfo.GetBuildingsSet()[i];

            // Create UI button for the building
            objectUI = Instantiate<GameObject>(ObjectUIPrefab, BuildingsListUI.position, BuildingsListUI.rotation).transform;
            objectUI.Find("Name").GetComponent<Text>().text = string.Format("{0}\n({1:0} coins)", buildingInfo.GetBuildingPool().tag, buildingInfo.GetPrice());
            objectUI.GetComponent<Button>().onClick.AddListener(() => Master.GetComponent<BuildObject>().StartBuilding(buildingInfo));
            objectUI.SetParent(BuildingsListUI, false);
        }
    }
}