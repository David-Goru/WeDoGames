using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the list of upgrades on UI
/// </summary>
public class UpgradesUI : UIList
{
    [SerializeField] Upgrades upgrades = null;
    [SerializeField] Transform activesUI = null;

    public override void Initialize(MasterInfo masterInfo, Transform masterObject)
    {
        loadUpgrades(masterInfo);
        activesUI = masterObject.GetComponent<ActivesUI>().ListUIObject;
    }

    void loadUpgrades(MasterInfo masterInfo)
    {
        foreach (Upgrade upgrade in masterInfo.UpgradesSet)
        {
            addUpgradeToUI(upgrade);
        }
    }

    void addUpgradeToUI(Upgrade upgrade)
    {
        Transform objectUI = Instantiate(ObjectUIPrefab, ListUIObject.position, ListUIObject.rotation).transform;
        objectUI.name = upgrade.name;
        objectUI.Find("Name").GetComponent<Text>().text = string.Format("{0}", upgrade.name);
        objectUI.Find("Cost").GetComponent<Text>().text = string.Format("{0} {1}", upgrade.Points, upgrade.Points > 1 ? "points" : "point");
        objectUI.GetComponent<Button>().onClick.AddListener(() => addUpgradeAction(upgrade));
        objectUI.GetComponent<HoverUIElement>().HoverText = upgrade.Description;
        objectUI.SetParent(ListUIObject, false);
        objectUI.gameObject.SetActive(false);
    }

    void addUpgradeAction(Upgrade upgrade)
    {
        if (MasterHandler.Instance.UpdatePoints(-upgrade.Points))
        {
            upgrades.AddUpgrade(upgrade);
            ListUIObject.Find(upgrade.name).gameObject.SetActive(false);
        }
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