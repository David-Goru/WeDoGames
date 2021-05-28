using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// This is a class
/// </summary>
public class ActivesUI : UIList
{
    [SerializeField] GameObject UIActivePrefab = null;

    List<Active> activesAvailable = null;
    List<Active> activesEnabled = null;

    public List<Active> ActivesAvailable { get => activesAvailable; }

    public override void Initialize(MasterInfo masterInfo, Transform masterObject)
    {
        activesAvailable = new List<Active>();
        activesEnabled = new List<Active>();
        loadUpgrades(masterInfo);
    }

    void loadUpgrades(MasterInfo masterInfo)
    {
        foreach (Upgrade upgrade in masterInfo.UpgradesSet)
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
        Transform objectUI = Instantiate(UIActivePrefab, ListUIObject.position, ListUIObject.rotation).transform;
        objectUI.name = upgrade.name;
        objectUI.Find("Name").GetComponent<Text>().text = string.Format("{0}", upgrade.name);
        objectUI.GetComponent<Button>().onClick.AddListener(() => MasterHandler.Instance.ActiveMode.SetActive(((Active)upgrade).ActiveAction));
        objectUI.GetComponent<HoverUIElement>().HoverText = upgrade.Description;
        objectUI.SetParent(ListUIObject, false);
        objectUI.gameObject.SetActive(false);

        ((Active)upgrade).ActiveAction.cooldownUI = objectUI.GetComponent<CooldownUI>();
    }

    public void EnableActive(Upgrade upgrade)
    {
        activesAvailable.Remove((Active)upgrade);
        activesEnabled.Add((Active)upgrade);
        ListUIObject.Find(upgrade.name).gameObject.SetActive(true);
    }
}