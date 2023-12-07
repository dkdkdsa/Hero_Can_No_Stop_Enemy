using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public delegate void ButtonClick(string userKey, FirebaseUserData userData);

public class AddFriendPanel : MonoBehaviour
{

    [SerializeField] private GameObject button;
    [SerializeField] private TMP_Text panelText;

    private FirebaseUserData userData;
    private string key;

    public event ButtonClick OnButtonClick;

    public void SetPanel(string panelText, bool buttonUse)
    {

        this.panelText.text = panelText;

        button.gameObject.SetActive(buttonUse);

    }

    public void SetUserDataAndKey(string key, FirebaseUserData userData) 
    {
        
        this.key = key;
        this.userData = userData;

    }

    public void ClickBtn()
    {

        OnButtonClick?.Invoke(key, userData);

    }

}
