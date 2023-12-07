using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{

    [SerializeField] private TowerListSO towerData;
    [SerializeField] private Transform shopSlotParent;
    [SerializeField] private Slot slotPrefab;

    private DeckSettingUIController controller;

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

        var tower = towerData.lists.Find(x => x.key == towerKey);

        if (tower != null)
        {

            if(FirebaseManager.Instance.userData.coin >= tower.price)
            {

                FirebaseManager.Instance.userData.coin -= tower.price;
                DeckManager.Instance.AbleTowerLs.Add(tower.key);
                controller.Refresh();

            }

        }    

    }
}