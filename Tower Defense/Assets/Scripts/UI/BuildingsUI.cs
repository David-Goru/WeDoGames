using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Initializes the UI buildings list
/// </summary>
public class BuildingsUI : UIList
{   
    public override void Initialize(MasterInfo masterInfo, Transform masterObject)
    {
        loadBuildings(masterInfo, masterObject);
    }

    public void UpdateBuildingInfo(BuildingInfo buildingInfo)
    {
        Transform objectUI = ListUIObject.Find(buildingInfo.name);
        setBuildingInfo(buildingInfo, objectUI);
    }

    void loadBuildings(MasterInfo masterInfo, Transform masterObject)
    {
        foreach (BuildingInfo buildingInfo in masterInfo.GetBuildingsSet())
        {
            addBuildingToUI(masterObject, buildingInfo);
        }
    }

    void addBuildingToUI(Transform masterObject, BuildingInfo buildingInfo)
    {
        Transform objectUI;
        objectUI = Instantiate(ObjectUIPrefab, ListUIObject.position, ListUIObject.rotation).transform;
        objectUI.name = buildingInfo.name;
        setBuildingInfo(buildingInfo, objectUI);
        objectUI.GetComponent<Button>().onClick.AddListener(() => masterObject.GetComponent<BuildObject>().StartBuilding(buildingInfo));
        objectUI.GetComponent<HoverUIElement>().HoverText = buildingInfo.Description;
        objectUI.SetParent(ListUIObject, false);
    }

    void setBuildingInfo(BuildingInfo buildingInfo, Transform objectUI)
    {
        objectUI.Find("Name").GetComponent<Text>().text = string.Format("{0}", buildingInfo.name);
        objectUI.Find("Cost").GetComponent<Text>().text = string.Format("{0} coins", buildingInfo.GetStat(StatType.PRICE));
    }
}