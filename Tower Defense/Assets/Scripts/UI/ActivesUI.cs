using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a class
/// </summary>
public class ActivesUI : UIList
{
    [SerializeField] GameObject UIActivePrefab = null;
    public override void Initialize(MasterInfo masterInfo, Transform masterObject)
    {
        loadUpgrades(masterInfo);
    }

    void loadUpgrades(MasterInfo masterInfo)
    {
        foreach (Upgrade upgrade in masterInfo.UpgradesSet)
        {
            if (upgrade is Active) addActiveToUI(upgrade);
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
        ListUIObject.Find(upgrade.name).gameObject.SetActive(true);
    }
}