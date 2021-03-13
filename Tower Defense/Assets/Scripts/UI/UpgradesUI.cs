using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a class
/// </summary>
public class UpgradesUI : UIList
{
    [SerializeField] Upgrades upgrades = null;

    /// <summary>
    /// Initializes the UI list
    /// </summary>
    /// <param name="masterInfo">Stores the upgrades list</param>
    /// <param name="masterObject">Transform that handles the upgrades system</param>
    public override void Initialize(MasterInfo masterInfo, Transform masterObject)
    {
        loadUpgrades(masterInfo);
    }

    /// <summary>
    /// Loads all upgrades from the upgrades set (from MasterInfo)
    /// </summary>
    /// <param name="masterInfo">Stores the upgrades list</param>
    /// <param name="masterObject">Transform that handles the upgrades system</param>
    void loadUpgrades(MasterInfo masterInfo)
    {
        foreach (Upgrade upgrade in masterInfo.UpgradesSet)
        {
            addUpgradeToUI( upgrade);
        }
    }

    /// <summary>
    /// Creates a button that calls "StartBuilding", sets the name and the price of that building and adds it to the UI
    /// </summary>
    /// <param name="masterObject">Transform that handles BuildObject</param>
    /// <param name="upgrade">Information about the building that will be displayed on the button</param>
    void addUpgradeToUI(Upgrade upgrade)
    {
        Transform objectUI = Instantiate(ObjectUIPrefab, ListUIObject.position, ListUIObject.rotation).transform;
        objectUI.Find("Name").GetComponent<Text>().text = string.Format("{0}", upgrade.name);
        objectUI.GetComponent<Button>().onClick.AddListener(() => addUpgradeAction(upgrade));
        objectUI.SetParent(ListUIObject, false);
    }

    void addUpgradeAction(Upgrade upgrade)
    {
        upgrades.AddUpgrade(upgrade);
        ListUIObject.Find(upgrade.name).gameObject.SetActive(false);
    }

    public void EnableRandomUpgrades(int amount)
    {
        ListUIObject.parent.parent.gameObject.SetActive(true);
        int tries = 100;
        while (amount > 0 && tries > 0)
        {
            GameObject upgrade = ListUIObject.GetChild(Random.Range(0, ListUIObject.childCount)).gameObject;
            if (!upgrade.activeSelf)
            {
                amount--;
                upgrade.SetActive(true);
            }
            tries--;
        }
    }

    public void CloseUpgrades()
    {
        foreach (Transform child in ListUIObject)
        {
            child.gameObject.SetActive(false);
        }
        ListUIObject.parent.parent.gameObject.SetActive(false);
    }
}