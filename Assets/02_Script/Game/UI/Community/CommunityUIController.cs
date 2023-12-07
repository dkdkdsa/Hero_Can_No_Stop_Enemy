using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommunityUIController : MonoBehaviour
{

    [SerializeField] private TMP_InputField userNameField;
    [SerializeField] private AddFriendPanel friendPreafab;

    public async void Serch()
    {

        var users = await FirebaseManager.Instance.GetAllUser();

        foreach (var user in users)
        {



        }

    }

}