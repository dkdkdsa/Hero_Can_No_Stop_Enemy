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

    private async void RefreshFriendAccept()
    {

        var friendReq = await FirebaseManager.Instance.GetFriendReq();
        var allUser = await FirebaseManager.Instance.GetAllUser();
        var friends = await FirebaseManager.Instance.GetFriendData(FirebaseManager.Instance.CurrentUserId);


        foreach (var item in friendReq.reqs)
        {

            var user = allUser.Find(x => x.key == item);

            if(user.userData != null && friends.friends.Find(x => x.userId == item) == null)
            {

                var slot = Instantiate(friendPreafab, acceptFirendParent);
                slot.SetPanel(user.userData.userName, true);
                slot.SetUserDataAndKey(item, user.userData);

            } 


        }

    }

    public async void Serch()
    {

        var users = await FirebaseManager.Instance.GetAllUser();

        var findUser = users.FindAll(x => x.userData.userName.Contains(userNameField.text));

        if(findUser.Count != 0)
        {

            foreach (var user in findUser)
            {

                var slot = Instantiate(friendPreafab, addFirendParent);

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