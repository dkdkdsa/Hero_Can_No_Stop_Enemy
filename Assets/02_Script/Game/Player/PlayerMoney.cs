using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{

    public int money { get; private set; } = 600;

    public void AddMoney(int value)
    {

        money += value;

    }

    public void SubtractMonmy(int value)
    {

        money -= value;

    }

}
