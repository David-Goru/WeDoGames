using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Initializes the UI turrets list
/// </summary>
public class TurretsUI : UIList
{
    List<KeyValuePair<Transform, BuildingInfo>> turretsSlots;
    Transform masterObject;

    public override void Initialize(MasterInfo masterInfo, Transform masterObject)
    {
        this.masterObject = masterObject;
        turretsSlots = new List<KeyValuePair<Transform, BuildingInfo>>();
        loadInitialSlots(masterInfo);
    }

    public void UpdateTurretInfo(BuildingInfo buildingInfo)
    {
        Transform objectUI = ListUIObject.Find(buildingInfo.name);
        setTurretInfo(objectUI, buildingInfo);
    }

    public void AddTurretToSlot(BuildingInfo buildingInfo, int slot)
    {
        if (slot >= turretsSlots.Count) return;

        turretsSlots[slot] = new KeyValuePair<Transform, BuildingInfo>(turretsSlots[slot].Key, buildingInfo);
        setTurretInfo(turretsSlots[slot].Key, turretsSlots[slot].Value);
    }

    void loadInitialSlots(MasterInfo masterInfo)
    {
        foreach (BuildingInfo buildingInfo in masterInfo.GetInitialTurretsSet())
        {
            turretsSlots.Add(new KeyValuePair<Transform, BuildingInfo>(addTurretToUI(buildingInfo), buildingInfo));
        }
    }

    Transform addTurretToUI(BuildingInfo buildingInfo)
    {
        Transform objectUI;
        objectUI = Instantiate(ObjectUIPrefab, ListUIObject.position, ListUIObject.rotation).transform;
        setTurretInfo(objectUI, buildingInfo);
        objectUI.SetParent(ListUIObject, false);

        return objectUI;
    }

    void setTurretInfo(Transform objectUI, BuildingInfo buildingInfo)
    {
        objectUI.name = buildingInfo.name;
        objectUI.GetComponent<Button>().onClick.AddListener(() => masterObject.GetComponent<BuildObject>().StartBuilding(buildingInfo));
        objectUI.GetComponent<HoverUIElement>().HoverText = buildingInfo.Description;
        objectUI.Find("Name").GetComponent<Text>().text = string.Format("{0}", buildingInfo.name);
        objectUI.Find("Cost").GetComponent<Text>().text = string.Format("{0} coins", buildingInfo.GetStat(StatType.PRICE));
    }
}