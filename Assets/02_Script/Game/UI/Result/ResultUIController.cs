using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if (IsHost)
        {

            HostSingle.Instance.GameManager.ShutdownAsync();

        }

        if(NetworkManager.Singleton.LocalClientId == dieClient)
        {

            resultText.text = "ÆÐ¹è";

        }
        else
        {

            resultText.text = "½Â¸®";

        }

    }

    public void GoTitle()
    {

        SceneManager.LoadScene(SceneList.MenuScene);

    }

}
