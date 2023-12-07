using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public delegate void ButtonClick();

public class AddFriendPanel : MonoBehaviour
{

    [SerializeField] private GameObject button;
    [SerializeField] private TMP_Text panelText;

    public event ButtonClick OnButtonClick;

    public void SetPanel(string panelText, bool buttonUse)
    {

        this.panelText.text = panelText;

        button.gameObject.SetActive(buttonUse);

    }

    public void ClickBtn()
    {

        OnButtonClick?.Invoke();

    }

}
