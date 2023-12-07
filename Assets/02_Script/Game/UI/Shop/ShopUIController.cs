using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{

    [Header("Data")]
    [SerializeField] private TowerListSO towerData;
    [SerializeField] private Transform shopSlotParent;
    [SerializeField] private Slot slotPrefab;

    [Header("BuyPanel")]
    [SerializeField] private GameObject panelObject;
    [SerializeField] private TMP_Text buyPanelText;

    private DeckSettingUIController controller;
    private string currentAbleTowerKey;

    private void Awake()
    {
        
        foreach(var item in towerData.lists)
        {

            if (!DeckManager.Instance.AbleTowerLs.Contains(item.key))
            {

                var slot = Instantiate(slotPrefab, shopSlotParent);
                slot.SetSlot(item.sprite, item.key, item.price.ToString());
                slot.OnPointerDownEvent += HandleSlotClick;

            }

        }

    }

    private void HandleSlotClick(string towerKey, Slot slot)
    {
  
        currentAbleTowerKey = towerKey;

    }

    public void SetPanel(string text)
    {

        panelObject.SetActive(true);
        buyPanelText.text = text;

    }

    public void RelesePanel()
    {

        panelObject.SetActive(false);

    }

    public void BuyTower()
    {

        var tower = towerData.lists.Find(x => x.key == currentAbleTowerKey);

        if (tower != null)
        {

            if (FirebaseManager.Instance.userData.coin >= tower.price)
            {

                FirebaseManager.Instance.userData.coin -= tower.price;
                DeckManager.Instance.AbleTowerLs.Add(tower.key);
                controller.Refresh();

            }

        }

    }

}