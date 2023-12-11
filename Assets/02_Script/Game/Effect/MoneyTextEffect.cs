using DG.Tweening;
using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyTextEffect : MonoBehaviour
{

    private TMP_Text text;

    private void Awake()
    {
        
        text = GetComponent<TMP_Text>();

    }

    public void SetText(int moneyValue)
    {

        text.text = $"+{moneyValue}$";

        transform.DOMoveY(transform.position.y + 2, 0.5f)
            .SetEase(Ease.Flash)
            .OnComplete(() =>
            {

                FAED.InsertPool(gameObject);

            });

    }

}
