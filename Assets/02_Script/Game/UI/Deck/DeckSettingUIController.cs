using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckSettingUIController : MonoBehaviour
{

    [SerializeField] private Transform deckMainPanel;
    [SerializeField] private Transform curDeckPanel;
    [SerializeField] private Slot slotPrefab;
    [SerializeField] private TowerListSO towerData;
    [SerializeField] private TMP_Text expText;

    private void Awake()
    {

        Refresh();

    }

    private void HandleSlotClick(string towerKey, Slot slot)
    {

        if(DeckManager.Instance.DeckLs.Find(x => x == towerKey) != null)
        {

            DeckManager.Instance.DeckLs.Remove(towerKey);
            slot.transform.SetParent(deckMainPanel);

        }
        else if(DeckManager.Instance.DeckLs.Count < 5)
        {


            DeckManager.Instance.DeckLs.Add(towerKey);
            slot.transform.SetParent(curDeckPanel);

        }

    }

    public void Refresh()
    {

        var curslot = curDeckPanel.GetComponentsInChildren<Slot>();
        var deckslot = deckMainPanel.GetComponentsInChildren<Slot>();

        foreach(var item in curslot)
        {

            Destroy(item.gameObject);

        }


        foreach (var item in deckslot)
        {

            Destroy(item.gameObject);

        }

        foreach (var tower in towerData.lists)
        {

            if (!DeckManager.Instance.AbleTowerLs.Contains(tower.key)) continue;

            var parent = DeckManager.Instance.DeckLs.Contains(tower.key) ? curDeckPanel : deckMainPanel;

            var slot = Instantiate(slotPrefab, parent);
            slot.SetSlot(tower.sprite, tower.key, tower.towerName);
            slot.OnPointerDownEvent += HandleSlotClick;

        }

    }

}
