using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommunityUIController : MonoBehaviour
{

    [Header("Data")]
    [SerializeField] private TMP_InputField userNameField;
    [SerializeField] private AddFriendPanel friendPreafab;

    [Header("Parent")]
    [SerializeField] private Transform addFirendParent;
    [SerializeField] private Transform acceptFirendParent;
    [SerializeField] private Transform friendParend;

    private void Start()
    {

        StartCoroutine(FriendReqRefreshCo());

    }

    private void RefreshFriendAccept()
    {



    }

    public async void Serch()
    {

        var users = await FirebaseManager.Instance.GetAllUser();

        var findUser = users.FindAll(x => x.userData.userName == userNameField.text);

        if(findUser.Count != 0)
        {

            foreach (var user in findUser)
            {

                var slot = Instantiate(friendPreafab);

                slot.SetPanel(user.userData.userName, true);
                slot.SetUserDataAndKey(user.key, user.userData);
                slot.OnButtonClick += HandleFriendReq;

            }

        }

    }
    private void HandleFriendReq(string userKey, FirebaseUserData userData)
    {

        FirebaseManager.Instance.SendFriendReq(userKey);

    }

    public IEnumerator FriendReqRefreshCo()
    {

        while (true)
        {

            yield return new WaitForSeconds(10f);
            RefreshFriendAccept();

        }

    }

}