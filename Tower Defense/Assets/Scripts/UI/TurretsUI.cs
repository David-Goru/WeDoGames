using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretsUI : UIList
{
    List<KeyValuePair<Transform, TurretInfo>> turretsSlots;
    KeyCode[] keys;
    int currentKey;

    void Start()
    {
        turretsSlots = new List<KeyValuePair<Transform, TurretInfo>>();
        keys = new KeyCode[3] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };
        currentKey = 0;
        loadInitialSlots();

        UI.Instance.TurretsUI = this;
    }

    public void UpdateTurretInfo(TurretInfo buildingInfo)
    {
        Transform objectUI = ListUIObject.Find(buildingInfo.name);
        if (objectUI != null) setTurretInfo(objectUI, buildingInfo);
    }

    public void UpdateTurretElement(TurretElement turretElement)
    {
        foreach (KeyValuePair<Transform, TurretInfo> turretSlot in turretsSlots)
        {
            if (turretSlot.Value.TurretElement == turretElement) setTurretInfo(turretSlot.Key, turretSlot.Value);
        }
    }

    public void AddTurretToSlot(TurretInfo buildingInfo, int slot)
    {
        if (slot >= turretsSlots.Count) return;

        turretsSlots[slot] = new KeyValuePair<Transform, TurretInfo>(turretsSlots[slot].Key, buildingInfo);
        setTurretInfo(turretsSlots[slot].Key, turretsSlots[slot].Value);
    }

    public bool ExistsTier(TurretTier turretTier)
    {
        foreach (KeyValuePair<Transform, TurretInfo> turretSlot in turretsSlots)
        {
            if (turretSlot.Value.TurretTier == turretTier) return true;
        }

        return false;
    }

    public bool ExistsElement(TurretElement turretElement)
    {
        foreach (KeyValuePair<Transform, TurretInfo> turretSlot in turretsSlots)
        {
            if (turretSlot.Value.TurretElement == turretElement) return true;
        }

        return false;
    }

    public bool ExistsTierAndElement(TurretTier turretTier, TurretElement turretElement)
    {
        foreach (KeyValuePair<Transform, TurretInfo> turretSlot in turretsSlots)
        {
            if (turretSlot.Value.TurretTier == turretTier && turretSlot.Value.TurretElement == turretElement) return true;
        }

        return false;
    }

    public bool ExistsTurret(TurretInfo turret)
    {
        foreach (KeyValuePair<Transform, TurretInfo> turretSlot in turretsSlots)
        {
            if (turretSlot.Value == turret) return true;
        }

        return false;
    }

    public int GetSlot(TurretElement turretElement, TurretTier turretTier)
    {
        foreach (KeyValuePair<Transform, TurretInfo> turretSlot in turretsSlots)
        {
            if (turretSlot.Value.TurretElement == turretElement && turretSlot.Value.TurretTier == turretTier) return turretsSlots.IndexOf(turretSlot);
        }

        return -1;
    }

    public void AddTurretUpgrade(TurretTransformation turretTransformation)
    {
        if (turretTransformation.TurretTier == TurretTier.SECOND)
        {
            for (int i = 0; i < turretsSlots.Count; i++)
            {
                if (turretsSlots[i].Value.TurretTier == TurretTier.FIRST)
                {
                    turretsSlots[i] = new KeyValuePair<Transform, TurretInfo>(turretsSlots[i].Key, turretTransformation.TurretInfo);
                    UI.UpdateKey(keys[i], () => Master.StartBuilding(turretsSlots[i].Value));
                    setTurretInfo(turretsSlots[i].Key, turretsSlots[i].Value);
                    break;
                }
            }
        }
        else if (turretTransformation.TurretTier == TurretTier.THIRD)
        {
            for (int i = 0; i < turretsSlots.Count; i++)
            {
                if (turretsSlots[i].Value.TurretTier == TurretTier.SECOND && turretsSlots[i].Value.TurretElement == turretTransformation.TurretElement)
                {
                    turretsSlots[i] = new KeyValuePair<Transform, TurretInfo>(turretsSlots[i].Key, turretTransformation.TurretInfo);
                    UI.UpdateKey(keys[i], () => Master.StartBuilding(turretsSlots[i].Value));
                    setTurretInfo(turretsSlots[i].Key, turretsSlots[i].Value);
                    break;
                }
            }
        }
    }

    void loadInitialSlots()
    {
        foreach (TurretInfo turretInfo in Master.Instance.MasterInfo.GetInitialTurretsSet())
        {
            turretsSlots.Add(new KeyValuePair<Transform, TurretInfo>(addTurretToUI(turretInfo), turretInfo));
        }
    }

    Transform addTurretToUI(TurretInfo turretInfo)
    {
        Transform objectUI = Instantiate(ObjectUIPrefab, ListUIObject.position, ListUIObject.rotation).transform;
        setTurretInfo(objectUI, turretInfo, (currentKey + 1).ToString());
        UI.AddKey(keys[currentKey], () => Master.StartBuilding(turretInfo));
        currentKey++;
        objectUI.SetParent(ListUIObject, false);

        return objectUI;
    }

    void setTurretInfo(Transform objectUI, TurretInfo turretInfo, string key = "None")
    {
        objectUI.name = turretInfo.name;
        objectUI.GetComponent<Button>().onClick.RemoveAllListeners();
        objectUI.GetComponent<Button>().onClick.AddListener(() => Master.StartBuilding(turretInfo));
        objectUI.GetComponent<HoverElement>().SetHoverText(turretInfo.Description);
        objectUI.Find("Name").GetComponent<Text>().text = string.Format("{0}", turretInfo.name);
        objectUI.Find("Cost").GetComponent<Text>().text = string.Format("{0}", turretInfo.GetStat(StatType.PRICE));
        if (key != "None") objectUI.Find("Key").GetComponent<Text>().text = string.Format("{0}", key);
        objectUI.Find("Icon").GetComponent<Image>().sprite = turretInfo.Icon;
    }
}