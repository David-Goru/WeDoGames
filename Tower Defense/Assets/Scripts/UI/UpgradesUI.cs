using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a class
/// </summary>
public class UpgradesUI : UIList
{
    /// <summary>
    /// Initializes the UI list
    /// </summary>
    /// <param name="masterInfo">Stores the upgrades list</param>
    /// <param name="masterObject">Transform that handles the upgrades system</param>
    public override void Initialize(MasterInfo masterInfo, Transform masterObject)
    {
        loadUpgrades(masterInfo, masterObject);
    }

    /// <summary>
    /// Loads all upgrades from the upgrades set (from MasterInfo)
    /// </summary>
    /// <param name="masterInfo">Stores the upgrades list</param>
    /// <param name="masterObject">Transform that handles the upgrades system</param>
    void loadUpgrades(MasterInfo masterInfo, Transform masterObject)
    {
        foreach (Upgrade upgrade in masterInfo.UpgradesSet)
        {
            addUpgradeToUI(masterObject, upgrade);
        }
    }

    /// <summary>
    /// Creates a button that calls "StartBuilding", sets the name and the price of that building and adds it to the UI
    /// </summary>
    /// <param name="masterObject">Transform that handles BuildObject</param>
    /// <param name="upgrade">Information about the building that will be displayed on the button</param>
    void addUpgradeToUI(Transform masterObject, Upgrade upgrade)
    {
        Transform objectUI = Instantiate(ObjectUIPrefab, ListUIObject.position, ListUIObject.rotation).transform;
        objectUI.Find("Name").GetComponent<Text>().text = string.Format("{0}", upgrade.Title);
        objectUI.GetComponent<Button>().onClick.AddListener(() => masterObject.GetComponent<Upgrades>().AddUpgrade(upgrade));
        objectUI.SetParent(ListUIObject, false);
    }
}