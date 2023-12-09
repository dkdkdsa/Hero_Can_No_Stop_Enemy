using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void SlotInteractionEvent(string towerKey, Slot slot);

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    private Image icon;
    private TMP_Text text;
    private string towerKey;

    public event SlotInteractionEvent OnPointerDownEvent;
    public event SlotInteractionEvent OnPointerUpEvent;
    public event SlotInteractionEvent OnPointerEnterEvent;
    public event SlotInteractionEvent OnPointerExitEvent;

    private void Awake()
    {

        icon = transform.Find("Icon").GetComponent<Image>();
        text = transform.Find("Text").GetComponent<TMP_Text>();

    }

    public void SetSlot(Sprite sprite, string towerKey, string txt)
    {

        icon.sprite = sprite;
        text.text = txt;
        this.towerKey = towerKey;

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        OnPointerDownEvent?.Invoke(towerKey, this);

    }

    public void OnPointerUp(PointerEventData eventData)
    {

        OnPointerUpEvent?.Invoke(towerKey, this);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        OnPointerEnterEvent?.Invoke(towerKey, this);

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        OnPointerExitEvent?.Invoke(towerKey, this);

    }
}
