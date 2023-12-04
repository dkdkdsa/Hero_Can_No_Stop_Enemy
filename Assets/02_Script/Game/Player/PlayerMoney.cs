using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnMoneyValueChangeEvent(int changeValue);

public class PlayerMoney : MonoBehaviour
{

    private int money = 600;

    public event OnMoneyValueChangeEvent OnMoneyChangeEvent;

    private void Start()
    {
        
        OnMoneyChangeEvent?.Invoke(money);

    }

    public void AddMoney(int value)
    {

        money += value;
        OnMoneyChangeEvent?.Invoke(money);

    }

    public void SubtractMonmy(int value)
    {

        money -= value;
        OnMoneyChangeEvent?.Invoke(money);

    }

    public int GetMoney() { return money; }

}
