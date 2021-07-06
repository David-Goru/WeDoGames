using UnityEngine;
using System.Collections.Generic;

public class ActivesUI : UIList
{
    List<Active> activesAvailable = null;
    List<Active> activesEnabled = null;

    public List<Active> ActivesAvailable { get => activesAvailable; }

    void Start()
    {
        activesAvailable = new List<Active>();
        activesEnabled = new List<Active>();
        loadUpgrades();

        UI.Instance.ActivesUI = this;
        hideUI();
    }

    public void EnableActive(Upgrade upgrade)
    {
        activesAvailable.Remove((Active)upgrade);
        activesEnabled.Add((Active)upgrade);
        ListUIObject.Find(upgrade.name).gameObject.SetActive(true);
        showUI();
    }

    void loadUpgrades()
    {
        foreach (Upgrade upgrade in Master.Instance.MasterInfo.UpgradesSet)
        {
            if (upgrade is Active)
            {
                activesAvailable.Add((Active)upgrade);
                addActiveToUI(upgrade);
            }
        }
    }

    void addActiveToUI(Upgrade upgrade)
    {
        Transform button = Instantiate(ObjectUIPrefab, ListUIObject.position, ListUIObject.rotation).transform;
        UI.SetButtonInfo(button, ListUIObject, upgrade.name, upgrade.Description, upgrade.Icon, () => Master.Instance.ActiveMode.SetActive(((Active)upgrade).ActiveAction));
        button.gameObject.SetActive(false);

        ((Active)upgrade).ActiveAction.cooldownUI = button.GetComponent<CooldownUI>();
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