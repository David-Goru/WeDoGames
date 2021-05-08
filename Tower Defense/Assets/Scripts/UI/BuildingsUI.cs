using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Initializes the UI buildings list
/// </summary>
public class BuildingsUI : UIList
{   
    /// <summary>
    /// Initializes the UI list
    /// </summary>
    /// <param name="masterInfo">Stores the buildings list</param>
    /// <param name="masterObject">Transform that handles BuildObject</param>
    public override void Initialize(MasterInfo masterInfo, Transform masterObject)
    {
        loadBuildings(masterInfo, masterObject);
    }

    /// <summary>
    /// Updates the current info of the building UI
    /// </summary>
    /// <param name="buildingInfo">Object with all the info of the building</param>
    public void UpdateBuildingInfo(BuildingInfo buildingInfo)
    {
        Transform objectUI = ListUIObject.Find(buildingInfo.name);
        setBuildingInfo(buildingInfo, objectUI);
    }

    /// <summary>
    /// Loads all buildings from the buildings set (from MasterInfo)
    /// </summary>
    /// <param name="masterInfo">Stores the buildings list</param>
    /// <param name="masterObject">Transform that handles BuildObject</param>
    void loadBuildings(MasterInfo masterInfo, Transform masterObject)
    {
        foreach (BuildingInfo buildingInfo in masterInfo.GetBuildingsSet())
        {
            addBuildingToUI(masterObject, buildingInfo);
        }
    }

    /// <summary>
    /// Creates a button that calls "StartBuilding", sets the name and the price of that building and adds it to the UI
    /// </summary>
    /// <param name="masterObject">Transform that handles BuildObject</param>
    /// <param name="buildingInfo">Information about the building that will be displayed on the button</param>
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

    /// <summary>
    /// Sets the building info on the UI
    /// </summary>
    /// <param name="buildingInfo">Object with all the info of the building</param>
    /// <param name="objectUI">Transform of the building UI</param>
    void setBuildingInfo(BuildingInfo buildingInfo, Transform objectUI)
    {
        objectUI.Find("Name").GetComponent<Text>().text = string.Format("{0}", buildingInfo.name);
        objectUI.Find("Cost").GetComponent<Text>().text = string.Format("{0} coins", buildingInfo.GetStat(StatType.PRICE));
    }
}