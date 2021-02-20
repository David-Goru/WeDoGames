using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Initializes the UI buildings list
/// </summary>
public class BuildingsUI : UIList
{
    // We don't like garbage
    Transform objectUI;
   
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
    /// Loads all buildings from the buildings set (from MasterInfo)
    /// </summary>
    /// <param name="masterInfo">Stores the buildings list</param>
    /// <param name="masterObject">Transform that handles BuildObject</param>
    void loadBuildings(MasterInfo masterInfo, Transform masterObject)
    {
        for (int i = 0; i < masterInfo.GetBuildingsSet().Length; i++)
        {
            addBuildingToUI(masterObject, masterInfo.GetBuildingsSet()[i]);
        }
    }

    /// <summary>
    /// Creates a button that calls "StartBuilding", sets the name and the price of that building and adds it to the UI
    /// </summary>
    /// <param name="masterObject">Transform that handles BuildObject</param>
    /// <param name="buildingInfo">Information about the building that will be displayed on the button</param>
    void addBuildingToUI(Transform masterObject, BuildingInfo buildingInfo)
    {
        objectUI = Instantiate(ObjectUIPrefab, ListUIObject.position, ListUIObject.rotation).transform;
        objectUI.Find("Name").GetComponent<Text>().text = string.Format("{0}\n({1:0} coins)", buildingInfo.GetBuildingPool().tag, buildingInfo.GetStat(StatType.PRICE));
        objectUI.GetComponent<Button>().onClick.AddListener(() => masterObject.GetComponent<BuildObject>().StartBuilding(buildingInfo));
        objectUI.SetParent(ListUIObject, false);
    }
}