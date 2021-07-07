using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretsUI : UIList
{
    List<KeyValuePair<Transform, TurretInfo>> turretsSlots;

    void Start()
    {
        turretsSlots = new List<KeyValuePair<Transform, TurretInfo>>();
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
                    setTurretInfo(turretsSlots[i].Key, turretsSlots[i].Value);
                    break;
                }
            }
        }
    }

    void loadInitialSlots()
    {
        foreach (TurretInfo buildingInfo in Master.Instance.MasterInfo.GetInitialTurretsSet())
        {
            turretsSlots.Add(new KeyValuePair<Transform, TurretInfo>(addTurretToUI(buildingInfo), buildingInfo));
        }
    }

    Transform addTurretToUI(TurretInfo buildingInfo)
    {
        Transform objectUI = Instantiate(ObjectUIPrefab, ListUIObject.position, ListUIObject.rotation).transform;
        setTurretInfo(objectUI, buildingInfo);
        objectUI.SetParent(ListUIObject, false);

        return objectUI;
    }

    void setTurretInfo(Transform objectUI, TurretInfo turretInfo)
    {
        objectUI.name = turretInfo.name;
        objectUI.GetComponent<Button>().onClick.AddListener(() => Master.StartBuilding(turretInfo));
        objectUI.GetComponent<HoverElement>().SetHoverText(turretInfo.Description);
        objectUI.Find("Name").GetComponent<Text>().text = string.Format("{0}", turretInfo.name);
        objectUI.Find("Cost").GetComponent<Text>().text = string.Format("{0} coins", turretInfo.GetStat(StatType.PRICE));
        objectUI.Find("Icon").GetComponent<Image>().sprite = turretInfo.Icon;
    }
}