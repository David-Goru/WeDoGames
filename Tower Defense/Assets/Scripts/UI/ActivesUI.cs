using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a class
/// </summary>
public class ActivesUI : UIList
{
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
    /// Loads all actives from the upgrades set (from MasterInfo)
    /// </summary>
    /// <param name="masterInfo">Stores the upgrades list</param>
    void loadUpgrades(MasterInfo masterInfo)
    {
        foreach (Upgrade upgrade in masterInfo.UpgradesSet)
        {
            if (upgrade is Active) addActiveToUI(upgrade);
        }
    }

    /// <summary>
    /// Creates a button that calls "activeAction", sets the name of the active and adds it to the UI
    /// </summary>
    /// <param name="upgrade">Information about the active that will be displayed on the button</param>
    void addActiveToUI(Upgrade upgrade)
    {
        Transform objectUI = Instantiate(ObjectUIPrefab, ListUIObject.position, ListUIObject.rotation).transform;
        objectUI.name = upgrade.name;
        objectUI.Find("Name").GetComponent<Text>().text = string.Format("{0}", upgrade.name);
        //Hay que cambiar la linea de abajo, ahora tiene que llamar a ActiveMode y pasarle la activa.
        //objectUI.GetComponent<Button>().onClick.AddListener(() => ((Active)upgrade).ActiveAction.UseActive());
        objectUI.SetParent(ListUIObject, false);
        objectUI.gameObject.SetActive(false);

        ((Active)upgrade).ActiveAction.UIElement = objectUI;
    }

    public void EnableActive(Upgrade upgrade)
    {
        ListUIObject.Find(upgrade.name).gameObject.SetActive(true);
    }
}