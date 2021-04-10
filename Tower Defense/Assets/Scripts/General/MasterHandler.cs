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

    [Header("References")]
    [SerializeField] MasterInfo masterInfo = null;
    [SerializeField] UpgradesUI upgradesUI;
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

        // If not testing without UI
        if (!testWithoutUI)
        {
            // Set the balance UI text
            balanceText.text = string.Format("{0} coins", MasterInfo.Balance);

            // Initialize lists
            foreach (UIList list in GetComponents(typeof(UIList)))
            {
                list.Initialize(MasterInfo, transform);
            }

            upgradesUI.EnableRandomUpgrades(2);
        }
    }

    /// <summary>
    /// Gets the current money of the player
    /// </summary>
    /// <returns>Returns the balance</returns>
    public float GetBalance() { return MasterInfo.Balance; }

    /// <summary>
    /// Checks if the player has enough money
    /// </summary>
    /// <param name="amount">Amount of money to check</param>
    /// <returns>Returns true if the player can afford it, false otherwise</returns>
    public bool CheckIfCanAfford(float amount) { return MasterInfo.Balance >= Mathf.Abs(amount); }

    /// <summary>
    /// Update the player balance with the amount given
    /// </summary>
    /// <param name="amount">Amount to add (or substract if passed as -X)</param>
    /// <returns>Returns false if the player doesn't have enough money</returns>
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