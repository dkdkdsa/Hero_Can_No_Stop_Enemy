using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeckController : MonoBehaviour
{

    [SerializeField] private Image playerDeckPanel;
    [SerializeField] private TowerListSO towerData;
    [SerializeField] private Slot slotPrefab;

    private void Awake()
    {
        
        foreach(var item in DeckManager.Instance.DeckLs)
        {

            var tower = towerData.lists.Find(x => x.key == item);
            var slot = Instantiate(slotPrefab, playerDeckPanel.transform);

            slot.SetSlot(tower.sprite, tower.key, tower.cost.ToString());
            slot.OnPointerDownEvent += HandleSlotClick;

        }

    }

    private void HandleSlotClick(string key, Slot slot)
    {



    }

}
