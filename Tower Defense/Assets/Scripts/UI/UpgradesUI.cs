using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Manages the list of upgrades on UI
/// </summary>
public class UpgradesUI : UIList
{
    [SerializeField] Upgrades upgrades = null;
    [SerializeField] Transform activesUI = null;

    public override void Initialize(MasterInfo masterInfo, Transform masterObject)
    {
        upgrades.UpgradesUI = this;
        activesUI = masterObject.GetComponent<ActivesUI>().ListUIObject;

        loadUpgrades(masterInfo);
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
        objectUI.Find("Cost").GetComponent<Text>().text = string.Format("{0} coins", upgrade.Price);
        objectUI.GetComponent<Button>().onClick.AddListener(() => addUpgradeAction(upgrade));
        objectUI.GetComponent<HoverUIElement>().HoverText = upgrade.Description;
        objectUI.SetParent(ListUIObject, false);
        objectUI.gameObject.SetActive(false);
        upgrade.ObjectUI = objectUI;
    }

    void addUpgradeAction(Upgrade upgrade)
    {
        if (MasterHandler.Instance.UpdateBalance(-upgrade.Price))
        {
            upgrades.AddUpgrade(upgrade);
            upgrade.ObjectUI.gameObject.SetActive(false);
        }
    }

    public void OpenUpgrades(int amount)
    {
        Time.timeScale = 0;

        List<Upgrade> upgradesAvailable = new List<Upgrade>();

        /* Add upgrades (to upgradesAvailable) */

        // Add nexus upgrades
        // Add tier 1 upgrades if there are still basic turrets
        // Add tier 2 upgrades for tier 1 turrets (if no tier 0 turrets)
        // Add tier 3 upgrades for tier 2 turrets (if no tier 1 turrets)
        // Add actives if !activesUI.Find(upgrade.name) || !activesUI.Find(upgrade.name).gameObject.activeSelf

        int amountOfUpgrades = amount > upgradesAvailable.Count ? upgradesAvailable.Count : amount;
        while (amountOfUpgrades > 0)
        {
            Upgrade upgrade = upgradesAvailable[Random.Range(0, upgradesAvailable.Count)];
            if (!upgrade.ObjectUI.gameObject.activeSelf)
            {
                amountOfUpgrades--;
                upgrade.ObjectUI.gameObject.SetActive(true);
            }
        }

        ListUIObject.parent.parent.gameObject.SetActive(true);
    }

    public void CloseUpgrades()
    {
        foreach (Transform child in ListUIObject)
        {
            child.gameObject.SetActive(false);
        }
        ListUIObject.parent.parent.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}