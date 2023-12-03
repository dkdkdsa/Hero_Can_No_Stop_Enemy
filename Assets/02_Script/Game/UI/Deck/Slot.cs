using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    private Image icon;
    private TMP_Text text;
    private string towerKey;

    private void Awake()
    {

        icon = transform.Find("Icon").GetComponent<Image>();
        text = transform.Find("Text").GetComponent<TMP_Text>();

    }

    public void SetSlot(Sprite sprite, string towerKey, int cost)
    {

        icon.sprite = sprite;
        text.text = cost.ToString();
        this.towerKey = towerKey;

    }

}
