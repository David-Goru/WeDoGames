using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterHandler : MonoBehaviour
{
    [Header("UI elements")]    
    public Text BalanceText;
    public static Text Balance;

    [Header("Other")]
    public MasterInfo MasterInfo;
    public static MasterInfo Info;

    void Start()
    {
        Balance = BalanceText;
        Info = MasterInfo;

        Balance.text = string.Format("{0} coins", Info.Balance);
    }

    public static float GetBalance() { return Info.Balance; }

    public static bool UpdateBalance(float amount)
    {
        // If reducing balance, check if balance > amount to take
        if (amount < 0 && Info.Balance < Mathf.Abs(amount)) return false;

        Info.Balance += amount;
        Balance.text = string.Format("{0} coins", Info.Balance);

        return true;
    }
}