using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the list of upgrades on UI
/// </summary>
public class UpgradesUI : UIList
{
    [SerializeField] Upgrades upgrades = null;
    [SerializeField] ActivesUI activesUI = null;
    [SerializeField] TurretsUI turretsUI = null;

    List<Upgrade> upgradesList = null;
    List<TurretTransformation> turretTransformations = null;
    List<TurretUpgrade> turretUpgrades = null;
    List<ElementalStatUpgrade> elementStatsUpgrades = null;

    public override void Initialize(MasterInfo masterInfo, Transform masterObject)
    {
        upgrades.UpgradesUI = this;
        upgradesList = new List<Upgrade>();
        activesUI = masterObject.GetComponent<ActivesUI>();
        turretsUI = masterObject.GetComponent<TurretsUI>();

        loadUpgrades(masterInfo);
    }

    public void OpenUpgrades(int amount)
    {
        Time.timeScale = 0;

        List<Upgrade> upgradesAvailable = new List<Upgrade>();

        // TODO: Add nexus upgrades

        if (turretsUI.ExistsTier(TurretTier.FIRST))
        {
            foreach (TurretTransformation upgrade in turretTransformations.FindAll(x => x.TurretTier == TurretTier.SECOND))
            {
                upgradesAvailable.Add(upgrade);
            }
        }
        else if (turretsUI.ExistsTier(TurretTier.SECOND))
        {
            foreach (TurretTransformation upgrade in turretTransformations.FindAll(x => x.TurretTier == TurretTier.THIRD))
            {
                if (turretsUI.ExistsTierAndElement(TurretTier.SECOND, upgrade.TurretElement) && !turretsUI.ExistsTurret(upgrade.TurretInfo)) upgradesAvailable.Add(upgrade);
            }
        }
        else if (turretsUI.ExistsTier(TurretTier.THIRD))
        {
            foreach (TurretUpgrade upgrade in turretUpgrades)
            {
                if (upgrade.CurrentUsages < upgrade.Usages && turretsUI.ExistsTurret(upgrade.TurretToUpgrade.TurretInfo)) upgradesAvailable.Add(upgrade);
            }
        }

        if (!turretsUI.ExistsTier(TurretTier.FIRST))
        {
            foreach (ElementalStatUpgrade upgrade in elementStatsUpgrades)
            {
                if (turretsUI.ExistsElement(upgrade.Element)) upgradesAvailable.Add(upgrade);
            }
        }

        foreach (Active active in activesUI.ActivesAvailable)
        {
            upgradesAvailable.Add(active);
        }

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

    void loadUpgrades(MasterInfo masterInfo)
    {
        foreach (Upgrade upgrade in masterInfo.UpgradesSet)
        {
            if (upgrade is TurretUpgrade) ((TurretUpgrade)upgrade).CurrentUsages = 0;
            upgradesList.Add(upgrade);
            addUpgradeToUI(upgrade);
        }

        turretTransformations = upgradesList.OfType<TurretTransformation>().ToList();
        turretUpgrades = upgradesList.OfType<TurretUpgrade>().ToList();
        elementStatsUpgrades = upgradesList.OfType<ElementalStatUpgrade>().ToList();
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
}