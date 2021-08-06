using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UpgradesUI : UIList
{
    [SerializeField] AudioClip clickSound = null;

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

        if (UI.ExistsTier(TurretTier.FIRST)) upgradesAvailable.AddRange(turretTransformations.FindAll(x => x.TurretTier == TurretTier.SECOND));
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
        Transform button = Instantiate(ObjectUIPrefab, ListUIObject.position, ListUIObject.rotation).transform;
        UI.SetButtonInfoWithCost(button, ListUIObject, upgrade.name, upgrade.Description, upgrade.Icon, upgrade.Price, () => addUpgradeAction(upgrade));
        button.gameObject.SetActive(false);
        upgrade.ObjectUI = button;
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
        else if (upgrade is ElementalStatUpgrade) enableElementalStatUpgrade((ElementalStatUpgrade)upgrade);
        else if (upgrade is TurretTransformation) UI.AddTurretUpgrade((TurretTransformation)upgrade);
        else if (upgrade is TurretUpgrade) enableTurretUpgrade((TurretUpgrade)upgrade);

        Master.Instance.RunSound(clickSound);
        CloseUpgrades();
    }

    void enableElementalStatUpgrade(ElementalStatUpgrade upgrade)
    {
        TurretInfo[] turrets = Master.Instance.MasterInfo.GetTurretsSet();
        foreach (TurretInfo turret in turrets)
        {
            if (turret.TurretElement == upgrade.Element) addStatsToTurret(upgrade.Stats.ToArray(), turret);
        }
        UI.UpdateTurretElement(upgrade.Element);
    }

    void enableTurretUpgrade(TurretUpgrade upgrade)
    {
        upgrade.CurrentUsages++;
        addStatsToTurret(upgrade.StatChanges, upgrade.TurretToUpgrade.TurretInfo);

        UI.UpdateTurretInfo(upgrade.TurretToUpgrade.TurretInfo);
    }

    void addStatsToTurret(Stat[] stats, TurretInfo turret)
    {
        foreach (Stat stat in stats)
        {
            turret.IncrementStat(stat.Type, stat.Value);
        }
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