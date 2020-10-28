using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterHandler : MonoBehaviour
{
    [Header("UI elements")]    
    public Text Balance;

    int balance;

    public int GetBalance() { return balance; }

    public bool UpdateBalance(int amount)
    {
        // If reducing balance, check if balance > amount to take
        if (amount < 0 && balance < Mathf.Abs(amount)) return false;

        balance += amount;
        Balance.text = string.Format("{0} coins", balance);
        
        return true;
    }
}