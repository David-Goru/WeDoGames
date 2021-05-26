using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] Text pointsText = null;

    [Header("References")]
    [SerializeField] MasterInfo masterInfo = null;
    [SerializeField] UpgradesUI upgradesUI = null;
    public ActiveMode ActiveMode;

    public Grid grid;

    // Store Master instance
    public static MasterHandler Instance;

    public MasterInfo MasterInfo { get => masterInfo; }

    /// <summary>
    /// Initiliazes the MasterHandler
    /// </summary>
    void Awake()
    {
        if (Instance == null) Instance = this;

        MasterInfo.InitializeVariables();

        if (GameManager.Instance != null) GameManager.Instance.AddCommand("addMoney", addMoney);

        // If not testing without UI
        if (!testWithoutUI)
        {
            // Set UI texts
            balanceText.text = string.Format("{0} coins", MasterInfo.Balance);
            pointsText.text = string.Format("{0} points", MasterInfo.Points);

            // Initialize lists
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

    public bool UpdatePoints(int amount)
    {
        if (amount < 0 && MasterInfo.Points < Mathf.Abs(amount)) return false;

        MasterInfo.Points += amount;
        if (pointsText != null) pointsText.text = string.Format("{0} points", MasterInfo.Points);

        return true;
    }
}