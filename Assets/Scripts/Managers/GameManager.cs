using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("Initial Balance")]
    float currentBalance = 2;

    // TODO: Move this to UIManager later
    [Header("UI Elements")]
    public Text currentBalanceText;
    private void Update()
    {
        currentBalanceText.text = "$" + currentBalance.ToString("#0.00");
    }

    public void AddCash(float amount)
    {
        if (amount < 0)
        {
            Debug.Log("Trying to add negative amount");
            return;
        }

        currentBalance += amount;
    }

    public void ExpendCash(float amount)
    {
        if (amount < 0)
        {
            Debug.Log("Trying to expend negative amount");
            return;
        }


        if (CanBuy(amount))
            currentBalance -= amount;
    }

    public bool CanBuy(float amount)
    {
        return currentBalance >= amount;
    }
}
