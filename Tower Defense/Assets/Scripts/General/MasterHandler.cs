using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to handle everything related to the main game loop
/// </summary>
public class MasterHandler : MonoBehaviour
{
    [Header("UI elements")]    
    public Text BalanceText;
    public static Text Balance;

    [Header("Other")]
    public MasterInfo MasterInfo;
    public static MasterInfo Info;

    /// <summary>
    /// Initiliazes the MasterHandler
    /// </summary>
    void Start()
    {
        Balance = BalanceText;
        Info = MasterInfo;

        // Set the balance UI text
        Balance.text = string.Format("{0} coins", Info.Balance);
    }

    /// <summary>
    /// Gets the current money of the player
    /// </summary>
    /// <returns>Returns the balance</returns>
    public static float GetBalance() { return Info.Balance; }

    /// <summary>
    /// Checks if the player has enough money
    /// </summary>
    /// <param name="amount">Amount of money to check</param>
    /// <returns>Returns true if the player can afford it, false otherwise</returns>
    public static bool CheckIfCanAffor(float amount) { return Info.Balance >= amount; }

    /// <summary>
    /// Update the player balance with the amount given
    /// </summary>
    /// <param name="amount">Amount to add (or substract if passed as -X)</param>
    /// <returns>Returns false if the player doesn't have enough money</returns>
    public static bool UpdateBalance(float amount)
    {
        // If reducing balance, check if balance > amount to take
        if (amount < 0 && Info.Balance < Mathf.Abs(amount)) return false;

        // Update balance and UI text
        Info.Balance += amount;
        Balance.text = string.Format("{0} coins", Info.Balance);

        return true;
    }
}