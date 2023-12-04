using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ResultUIController : NetworkBehaviour
{

    [SerializeField] private TMP_Text resultText;

    public override void OnNetworkSpawn()
    {

        if (IsServer)
        {

            ulong dieClient = (ulong)PlayerPrefs.GetInt("DieClient");
            SetTextClientRPC(dieClient);

        }

    }

    [ClientRpc]
    private void SetTextClientRPC(ulong dieClient)
    {

        if(NetworkManager.Singleton.LocalClientId == dieClient)
        {

            resultText.text = "ÆÐ¹è";

        }
        else
        {

            resultText.text = "½Â¸®";

        }

    }

}
