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
    [SerializeField] private Transform friendParent;

    private void Start()
    {

        RefreshFriendAccept();
        RefreshFriend();
        StartCoroutine(RefreshCo());

    }

    private async void RefreshFriendAccept()
    {

        var friendReq = await FirebaseManager.Instance.GetFriendReq(FirebaseManager.Instance.CurrentUserId);
        var allUser = await FirebaseManager.Instance.GetAllUser();
        var friends = await FirebaseManager.Instance.GetFriendData(FirebaseManager.Instance.CurrentUserId);

        var slots = acceptFirendParent.GetComponentsInChildren<AddFriendPanel>();

        foreach(var slot in slots)
        {

            Destroy(slot.gameObject);

        }

        foreach (var item in friendReq.reqs)
        {

            var user = allUser.Find(x => x.key == item);

            if(user.userData != null && friends.friends.Find(x => x.userId == item) == null)
            {

                var slot = Instantiate(friendPreafab, acceptFirendParent);
                slot.SetPanel(user.userData.userName, true);
                slot.SetUserDataAndKey(item, user.userData);
                slot.SetBtnText("수락");
                slot.OnButtonClick += HandleFriendAccept;

            } 


        }

    }
    private async void RefreshFriend()
    {

        var friends = await FirebaseManager.Instance.GetFriendData(FirebaseManager.Instance.CurrentUserId);

        var slots = friendParent.GetComponentsInChildren<AddFriendPanel>();

        foreach (var slot in slots)
        {

            Destroy(slot.gameObject);

        }

        foreach(var friend in friends.friends)
        {

            var slot = Instantiate(friendPreafab, friendParent);
            slot.SetPanel(friend.userName, false);

        }


    }

    public async void Serch()
    {

        var slots = addFirendParent.GetComponentsInChildren<AddFriendPanel>();

        foreach (var slot in slots)
        {

            Destroy(slot.gameObject);

        }

        var users = await FirebaseManager.Instance.GetAllUser();


        var findUser = users.FindAll(
            x => x.userData.userName.Contains(userNameField.text) 
        && x.key != FirebaseManager.Instance.CurrentUserId);

        if(findUser.Count != 0)
        {

            foreach (var user in findUser)
            {

                var reqs = await FirebaseManager.Instance.GetFriendReq(user.key);

                if (reqs.reqs.Contains(FirebaseManager.Instance.CurrentUserId)) continue;

                var slot = Instantiate(friendPreafab, addFirendParent);

                slot.SetPanel(user.userData.userName, true);
                slot.SetUserDataAndKey(user.key, user.userData);
                slot.SetBtnText("친구요청");
                slot.OnButtonClick += HandleFriendReq;

            }

        }

    }
    private async void HandleFriendReq(string userKey, FirebaseUserData userData)
    {

        await FirebaseManager.Instance.SendFriendReq(userKey);
        Serch();

    }

    private async void HandleFriendAccept(string userKey, FirebaseUserData userData)
    {

        await FirebaseManager.Instance.AddFriend(userKey,
            new FirebaseFriend
            {

                userId = FirebaseManager.Instance.CurrentUserId,
                userName = FirebaseManager.Instance.userData.userName,

            });

        await FirebaseManager.Instance.AddFriend(FirebaseManager.Instance.CurrentUserId, new FirebaseFriend
        {

            userId = userKey,
            userName = userData.userName,

        });

        RefreshFriendAccept();
        RefreshFriend();

    }

    public IEnumerator RefreshCo()
    {

        while (true)
        {

            yield return new WaitForSeconds(10f);
            RefreshFriendAccept();
            RefreshFriend();

        }

    }

}