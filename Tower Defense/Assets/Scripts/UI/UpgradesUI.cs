using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UpgradesUI : UIList
{
    List<Upgrade> upgradesList = null;
    List<TurretTransformation> turretTransformations = null;
    List<TurretUpgrade> turretUpgrades = null;
    List<ElementalStatUpgrade> elementStatsUpgrades = null;

    void Start()
    {
        upgradesList = new List<Upgrade>();
        loadUpgrades();

        UI.Instance.UpgradesUI = this;
        hideUI();
    }

    public void OpenUpgrades(int amount)
    {
        Time.timeScale = 0;

        List<Upgrade> upgradesAvailable = new List<Upgrade>();

        // TODO: Add nexus upgrades

        if (UI.ExistsTier(TurretTier.FIRST))
        {
            foreach (TurretTransformation upgrade in turretTransformations.FindAll(x => x.TurretTier == TurretTier.SECOND))
            {
                upgradesAvailable.Add(upgrade);
            }
        }
        else if (UI.ExistsTier(TurretTier.SECOND))
        {
            foreach (TurretTransformation upgrade in turretTransformations.FindAll(x => x.TurretTier == TurretTier.THIRD))
            {
                if (UI.ExistsTierAndElement(TurretTier.SECOND, upgrade.TurretElement) && !UI.ExistsTurret(upgrade.TurretInfo)) upgradesAvailable.Add(upgrade);
            }
        }
        else if (UI.ExistsTier(TurretTier.THIRD))
        {
            foreach (TurretUpgrade upgrade in turretUpgrades)
            {
                if (upgrade.CurrentUsages < upgrade.Usages && UI.ExistsTurret(upgrade.TurretToUpgrade.TurretInfo)) upgradesAvailable.Add(upgrade);
            }
        }

        if (!UI.ExistsTier(TurretTier.FIRST))
        {
            foreach (ElementalStatUpgrade upgrade in elementStatsUpgrades)
            {
                if (UI.ExistsElement(upgrade.Element)) upgradesAvailable.Add(upgrade);
            }
        }

        upgradesAvailable.AddRange(UI.GetAvailableActives());

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

        showUI();
    }

    public void CloseUpgrades()
    {
        foreach (Transform child in ListUIObject)
        {
            child.gameObject.SetActive(false);
        }
        hideUI();
        Time.timeScale = 1;
    }

    void loadUpgrades()
    {
        foreach (Upgrade upgrade in Master.Instance.MasterInfo.UpgradesSet)
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
        objectUI.GetComponent<HoverElement>().HoverText = upgrade.Description;
        objectUI.SetParent(ListUIObject, false);
        objectUI.gameObject.SetActive(false);
        upgrade.ObjectUI = objectUI;
    }

    void addUpgradeAction(Upgrade upgrade)
    {
        if (Master.Instance.UpdateBalance(-upgrade.Price))
        {
            addUpgrade(upgrade);
            upgrade.ObjectUI.gameObject.SetActive(false);
        }
    }

    void addUpgrade(Upgrade upgrade)
    {
        if (upgrade is Active) UI.EnableActive(upgrade);
        else if (upgrade is ElementalStatUpgrade)
        {
            ElementalStatUpgrade elementalStatUpgrade = (ElementalStatUpgrade)upgrade;
            BuildingInfo[] turrets = Master.Instance.MasterInfo.GetTurretsSet();
            foreach (BuildingInfo turret in turrets)
            {
                if (turret.TurretElement == elementalStatUpgrade.Element)
                {
                    foreach (Stat stat in elementalStatUpgrade.Stats)
                    {
                        turret.IncrementStat(stat.Type, stat.Value);
                    }
                }
            }

            UI.UpdateTurretElement(elementalStatUpgrade.Element);
        }
        else if (upgrade is TurretTransformation)
        {
            UI.AddTurretUpgrade((TurretTransformation)upgrade);
        }
        else if (upgrade is TurretUpgrade)
        {
            TurretUpgrade turretUpgrade = (TurretUpgrade)upgrade;
            turretUpgrade.CurrentUsages++;

            foreach (Stat stat in turretUpgrade.StatChanges)
            {
                turretUpgrade.TurretToUpgrade.TurretInfo.IncrementStat(stat.Type, stat.Value);
            }

            UI.UpdateTurretInfo(turretUpgrade.TurretToUpgrade.TurretInfo);
        }

        CloseUpgrades();
    }

    void showUI()
    {
        changeVisibility(true);
    }

    void hideUI()
    {
        changeVisibility(false);
    }

    void changeVisibility(bool visible)
    {
        gameObject.SetActive(visible);
    }
}