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

        controller = FindObjectOfType<DeckSettingUIController>();

        Refresh();

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

    private async void UpdatePanel()
    {

        var tower = towerData.lists.Find(x => x.key == currentAbleTowerKey);

        if (tower == null) return;

        var discountList = await FirebaseManager.Instance.GetDiscountTower();
        var isDiscount = discountList.Contains(tower.key);

        var price = isDiscount ? tower.price / 2 : tower.price;

        SetPanel($"{tower.towerName}을/를 {price}$에 구매하시겠습니까?");

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

    public async void BuyTower()
    {

        var tower = towerData.lists.Find(x => x.key == currentAbleTowerKey);
        var discountList = await FirebaseManager.Instance.GetDiscountTower();

        if (tower != null)
        {

            var isDiscount = discountList.Contains(tower.key);

            var price = isDiscount ? tower.price / 2 : tower.price;

            if (FirebaseManager.Instance.userData.coin >= price)
            {

                FirebaseManager.Instance.userData.coin -= price;
                DeckManager.Instance.AbleTowerLs.Add(tower.key);
                controller.Refresh();
                Refresh();
                RelesePanel();

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

    public async void Refresh()
    {

        var slots = shopSlotParent.GetComponentsInChildren<Slot>();
        var discountList = await FirebaseManager.Instance.GetDiscountTower();

        foreach(var slot in slots)
        {

            Destroy(slot.gameObject);

        }

        foreach (var item in towerData.lists)
        {

            if (!DeckManager.Instance.AbleTowerLs.Contains(item.key))
            {

                var isDiscount = discountList.Contains(item.key);

                var text = isDiscount ? $"할인! {item.price / 2}" : item.price.ToString();

                var slot = Instantiate(slotPrefab, shopSlotParent);
                slot.SetSlot(item.sprite, item.key, text);
                slot.OnPointerDownEvent += HandleSlotClick;

            }

        }

    }

}