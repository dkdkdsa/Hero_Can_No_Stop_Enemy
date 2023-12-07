using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityUIController : MonoBehaviour
{
    
    public async void Serch()
    {

        var users = await FirebaseManager.Instance.GetAllUser();

    }

}