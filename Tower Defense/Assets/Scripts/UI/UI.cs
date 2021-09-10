using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class UI : MonoBehaviour
{
    [System.NonSerialized] public EntityInfoUI EntityInfo;
    [System.NonSerialized] public HoverUI HoverUI;
    [System.NonSerialized] public ActivesUI ActivesUI;
    [System.NonSerialized] public TurretsUI TurretsUI;
    [System.NonSerialized] public UpgradesUI UpgradesUI;
    [System.NonSerialized] public GeneralInfoUI GeneralInfoUI;
    [System.NonSerialized] public Chat Chat;

    List<KeyBinding> keyBindings;
    public List<KeyBinding> KeyBindings { get => keyBindings; set => keyBindings = value; }

    public static UI Instance;

    void Start()
    {
        Instance = this;
        keyBindings = new List<KeyBinding>();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        foreach (KeyBinding keyBinding in keyBindings) keyBinding.Check();
    }

    public static void AddKey(KeyCode key, UnityAction action)
    {
        if (Instance == null) return;

        Instance.KeyBindings.Add(new KeyBinding(key, action));
    }

    public static void UpdateKey(KeyCode key, UnityAction action)
    {
        if (Instance == null) return;

        Instance.KeyBindings.Find(x => x.Key == key).Action = action;
    }

    public static void UpdateUI()
    {
        if (Instance == null) return;

        if (Instance.Chat != null) Instance.Chat.CheckPlayerInput();
    }

    public static void ShowEntityInfoUI(Entity entity)
    {
        if (Instance == null) return;

        if (Instance.EntityInfo != null) Instance.EntityInfo.ShowUI(entity);
    }

    public static void ShowHoverUI(string text)
    {
        if (Instance == null) return;

        if (Instance.HoverUI != null) Instance.HoverUI.ShowUI(text);
    }

    public static void HideHoverUI()
    {
        if (Instance == null) return;

        if (Instance.HoverUI != null) Instance.HoverUI.HideUI();
    }

    public static List<Active> GetAvailableActives()
    {
        if (Instance == null) return new List<Active>();

        if (Instance.ActivesUI != null) return Instance.ActivesUI.ActivesAvailable;
        return new List<Active>();
    }

    public static void EnableActive(Upgrade upgrade)
    {
        if (Instance == null) return;

        if (Instance.ActivesUI != null) Instance.ActivesUI.EnableActive(upgrade);
    }

    public static void UpdateTurretElement(TurretElement element)
    {
        if (Instance == null) return;

        if (Instance.TurretsUI != null) Instance.TurretsUI.UpdateTurretElement(element);
    }

    public static void AddTurretUpgrade(Upgrade upgrade)
    {
        if (Instance == null) return;

        if (Instance.TurretsUI != null) Instance.TurretsUI.AddTurretUpgrade((TurretTransformation)upgrade);
    }

    public static void UpdateTurretInfo(TurretInfo buildingInfo)
    {
        if (Instance == null) return;

        if (Instance.TurretsUI != null) Instance.TurretsUI.UpdateTurretInfo(buildingInfo);
    }

    public static bool ExistsTier(TurretTier tier)
    {
        if (Instance == null) return false;

        if (Instance.TurretsUI != null) return Instance.TurretsUI.ExistsTier(tier);
        return false;
    }

    public static bool ExistsElement(TurretElement element)
    {
        if (Instance == null) return false;

        if (Instance.TurretsUI != null) return Instance.TurretsUI.ExistsElement(element);
        return false;
    }

    public static bool ExistsTierAndElement(TurretTier tier, TurretElement element)
    {
        if (Instance == null) return false;

        if (Instance.TurretsUI != null) return Instance.TurretsUI.ExistsTierAndElement(tier, element);
        return false;
    }

    public static bool ExistsTurret(TurretInfo buildingInfo)
    {
        if (Instance == null) return false;

        if (Instance.TurretsUI != null) return Instance.TurretsUI.ExistsTurret(buildingInfo);
        return false;
    }

    public static void OpenUpgrades(int amount)
    {
        if (Instance == null) return;

        if (Instance.UpgradesUI != null) Instance.UpgradesUI.OpenUpgrades(amount);
    }

    public static void CloseUpgrades()
    {
        if (Instance == null) return;

        if (Instance.UpgradesUI != null) Instance.UpgradesUI.CloseUpgrades();
    }

    public static void ForceCloseUpgrades()
    {
        if (Instance == null) return;

        if (Instance.UpgradesUI != null) Instance.UpgradesUI.ForceCloseUpgrades();
    }

    public static void UpdateBalanceText(int balance)
    {
        if (Instance == null) return;

        if (Instance.GeneralInfoUI != null) Instance.GeneralInfoUI.UpdateBalanceText(balance);
    }

    public static void UpdateNexusLifeText(int nexusLife)
    {
        if (Instance == null) return;

        if (Instance.GeneralInfoUI != null) Instance.GeneralInfoUI.UpdateNexusLifeText(nexusLife);
    }

    public static void UpdateWaveText(int wave)
    {
        if (Instance == null) return;

        if (Instance.GeneralInfoUI != null) Instance.GeneralInfoUI.UpdateWaveText(wave);
    }

    public static void UpdateWaveTimerText(int secondsRemaining)
    {
        if (Instance == null) return;

        if (Instance.GeneralInfoUI != null) Instance.GeneralInfoUI.UpdateWaveTimerText(secondsRemaining);
    }

    public static void AddChatCommand(string commandName, UnityAction<string[]> commandAction)
    {
        if (Instance == null) return;

        Instance.StartCoroutine(Instance.LateAddChatCommand(commandName, commandAction));
    }

    public IEnumerator LateAddChatCommand(string commandName, UnityAction<string[]> commandAction)
    {
        yield return new WaitForSeconds(0.5f);
        if (Instance.Chat != null) Instance.Chat.AddCommand(commandName, commandAction);
    }

    public static void SetButtonInfo(Transform button, Transform parent, string name, string description, Sprite icon, UnityAction action, string type = null)
    {
        Text nameText = button.Find("Name").GetComponent<Text>();
        Image iconImage = button.Find("Icon").GetComponent<Image>();
        HoverElement hoverElement = button.GetComponent<HoverElement>();
        Button buttonComponent = button.GetComponent<Button>();

        button.name = name;
        nameText.text = name;
        if (type != null) button.Find("Type").GetComponent<Text>().text = type;
        iconImage.sprite = icon;
        buttonComponent.onClick.AddListener(action);
        if (hoverElement) hoverElement.HoverText = description;
        button.SetParent(parent, false);
    }
}