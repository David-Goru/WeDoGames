using UnityEngine;
using UnityEngine.UI;

public class Master : MonoBehaviour
{
    [Header("References")]
    [SerializeField] MasterInfo masterInfo = null;

    public ActiveMode ActiveMode;
    public Grid grid;
    public MasterInfo MasterInfo { get => masterInfo; }
    [System.NonSerialized] public BuildObject BuildObject;

    public static Master Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;

        MasterInfo.InitializeVariables();

        UI.AddChatCommand("addMoney", addMoney);
    }

    void Update()
    {
        UI.UpdateUI();
    }

    public float GetBalance() { return MasterInfo.Balance; }

    public bool CheckIfCanAfford(float amount) { return MasterInfo.Balance >= Mathf.Abs(amount); }

    public bool UpdateBalance(float amount)
    {
        if (amount < 0 && !CheckIfCanAfford(amount)) return false;

        MasterInfo.Balance += amount;
        UI.UpdateBalanceText(Mathf.RoundToInt(MasterInfo.Balance));

        return true;
    }

    public static void StartBuilding(BuildingInfo buildingInfo)
    {
        if (Instance == null) return;

        if (Instance.BuildObject != null) Instance.BuildObject.StartBuilding(buildingInfo);
    }

    public void StopBuilding()
    {
        BuildObject.StopBuilding();
    }

    void addMoney(string[] parameters)
    {
        if (parameters.Length == 0) return;

        int money;
        int.TryParse(parameters[0], out money);

        UpdateBalance(money);
    }
}