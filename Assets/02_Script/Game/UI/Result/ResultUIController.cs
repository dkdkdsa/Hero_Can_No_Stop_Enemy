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
            StartCoroutine(AutoShotdownCo());

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

    public void GoTitle()
    {


        if (IsHost)
        {

            HostSingle.Instance.GameManager.ShutdownAsync();

        }

        SceneManager.LoadScene(SceneList.MenuScene);

    }

    private IEnumerator AutoShotdownCo()
    {

        yield return new WaitForSeconds(1);

        if (IsHost)
        {

            HostSingle.Instance.GameManager.ShutdownAsync();

        }

    }

}
