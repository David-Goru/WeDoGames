using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to handle everything related to the main game loop
/// </summary>
public class MasterHandler : MonoBehaviour
{
    [Header("Tools"), Tooltip("So we don't need anything UI related")]
    [SerializeField] bool testWithoutUI = false;

    [Header("UI elements")]
    [SerializeField] Text balanceText = null;

    [Header("References")]
    [SerializeField] MasterInfo masterInfo = null;

    public ActiveMode ActiveMode;
    public Grid grid;
    public MasterInfo MasterInfo { get => masterInfo; }

    public static MasterHandler Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;

        MasterInfo.InitializeVariables();

        if (Chat.Instance != null) Chat.Instance.AddCommand("addMoney", addMoney);

        if (!testWithoutUI)
        {
            balanceText.text = string.Format("{0} coins", MasterInfo.Balance);

            foreach (UIList list in GetComponents(typeof(UIList)))
            {
                list.Initialize(MasterInfo, transform);
            }
        }
    }

    private void addMoney(string[] parameters)
    {
        if (parameters.Length == 0) return; 

        int money;
        int.TryParse(parameters[0], out money);

        UpdateBalance(money);
    }

    public float GetBalance() { return MasterInfo.Balance; }

    public bool CheckIfCanAfford(float amount) { return MasterInfo.Balance >= Mathf.Abs(amount); }

    public bool UpdateBalance(float amount)
    {
        // If reducing balance, check if balance > amount to take
        if (amount < 0 && !CheckIfCanAfford(amount)) return false;

        // If not testing without UI (Balance will be null if TestingWithoutUI is enabled)
        if (balanceText != null)
        {
            // Update balance and UI text
            MasterInfo.Balance += amount;
            balanceText.text = string.Format("{0} coins", MasterInfo.Balance);
        }

        return true;
    }
}