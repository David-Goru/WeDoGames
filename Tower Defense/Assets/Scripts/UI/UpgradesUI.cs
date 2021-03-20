using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a class
/// </summary>
public class UpgradesUI : UIList
{
    [SerializeField] Upgrades upgrades = null;
    [SerializeField] Transform activesUI = null;

    /// <summary>
    /// Initializes the UI list
    /// </summary>
    /// <param name="masterInfo">Stores the upgrades list</param>
    /// <param name="masterObject">Transform that handles the upgrades system</param>
    public override void Initialize(MasterInfo masterInfo, Transform masterObject)
    {
        loadUpgrades(masterInfo);
        activesUI = masterObject.GetComponent<ActivesUI>().ListUIObject;
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
            addUpgradeToUI(upgrade);
        }
    }

    /// <summary>
    /// Creates a button that calls "addUpgradeAction", sets the name of the upgrade and adds it to the UI
    /// </summary>
    /// <param name="upgrade">Information about the upgrade that will be displayed on the button</param>
    void addUpgradeToUI(Upgrade upgrade)
    {
        Transform objectUI = Instantiate(ObjectUIPrefab, ListUIObject.position, ListUIObject.rotation).transform;
        objectUI.name = upgrade.name;
        objectUI.Find("Name").GetComponent<Text>().text = string.Format("{0}", upgrade.name);
        objectUI.GetComponent<Button>().onClick.AddListener(() => addUpgradeAction(upgrade));
        objectUI.SetParent(ListUIObject, false);
        objectUI.gameObject.SetActive(false);
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
                if (!activesUI.Find(upgrade.name) || !activesUI.Find(upgrade.name).gameObject.activeSelf)
                {
                    amount--;
                    upgrade.SetActive(true);
                }
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