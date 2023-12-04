using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMoneyViwer : MonoBehaviour
{

    [SerializeField] private TMP_Text moneyText;

    private PlayerMoney playerMoney;

    private void Awake()
    {
        
        playerMoney = FindObjectOfType<PlayerMoney>();
        playerMoney.OnMoneyChangeEvent += HandleMoneyChanged;

    }

    private void HandleMoneyChanged(int changeValue)
    {

        moneyText.text = $"{changeValue}$";

    }

}
