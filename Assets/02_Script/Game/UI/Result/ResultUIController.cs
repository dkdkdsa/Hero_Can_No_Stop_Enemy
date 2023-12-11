using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultUIController : NetworkBehaviour
{

    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text resCoin;

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

            resultText.text = "�й�";
            resCoin.text = "300���� ȹ��!";
            FirebaseManager.Instance.userData.coin += 300;

        }
        else
        {

            resultText.text = "�¸�";
            resCoin.text = "1000���� ȹ��!";
            FirebaseManager.Instance.userData.coin += 1000;

        }

        FirebaseManager.Instance.SaveUserData();

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
