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

        var findUser = users.FindAll(x => x.userData.userName == userNameField.text);

        if(findUser.Count != 0)
        {

            foreach (var user in findUser)
            {

                var slot = Instantiate(friendPreafab);

                slot.SetPanel(user.userData.userName, true);
                slot.SetUserDataAndKey(user.key, user.userData);

            }

        }

    }

}