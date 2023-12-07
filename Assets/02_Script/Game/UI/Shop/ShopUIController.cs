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
    [SerializeField] private TMP_Text coinText;

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

        controller = FindObjectOfType<DeckSettingUIController>();

    }

    private void Update()
    {

        coinText.text = $"코인 {FirebaseManager.Instance.userData.coin}";

    }

    private void HandleSlotClick(string towerKey, Slot slot)
    {
  
        currentAbleTowerKey = towerKey;
        UpdatePanel();

    }

    private void UpdatePanel()
    {

        var tower = towerData.lists.Find(x => x.key == currentAbleTowerKey);

        if (tower == null) return;

        SetPanel($"{tower.towerName}을/를 {tower.price}$에 구매하시겠습니까?");

    }

    private void SetPanel(string text)
    {

        panelObject.SetActive(true);
        buyPanelText.text = text;

    }

    private void RelesePanel()
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
            else
            {

                SetPanel("코인이 부족합니다!");

            }

        }

    }

    public void BuyCancel()
    {

        RelesePanel();

    }

}