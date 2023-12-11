using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneUIController : NetworkBehaviour
{

    [SerializeField] private TMP_Text loadingText;

    private void Awake()
    {

        HostSingle.Instance.GameManager.OnPlayerConnect += HandlePlayerConnect;

    }

    public override void OnNetworkSpawn()
    {

        if (IsHost)
        {

            HostSingle.Instance.GameManager.OnPlayerDisconnect += HandlePlayerDisconnect;

        }

        if (IsClient && !IsServer)
        {

            NetworkManager.Singleton.OnClientStopped += HandleDisconnect;

        }

    }

    private void HandlePlayerDisconnect(string userName, ulong clid)
    {

        HostSingle.Instance.GameManager.ShutdownAsync();
        SceneManager.LoadScene(SceneList.MenuScene);

    }

    private void HandleDisconnect(bool obj)
    {

        SceneManager.LoadScene(SceneList.MenuScene);

    }

    private void Update()
    {

#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Space))
        {

            NetworkManager.Singleton.SceneManager.LoadScene(SceneList.GameScene, LoadSceneMode.Single);

        }

#endif

    }

    public override void OnDestroy()
    {

        base.OnDestroy();

        HostSingle.Instance.GameManager.OnPlayerConnect -= HandlePlayerConnect;

        if (IsHost)
        {

            HostSingle.Instance.GameManager.OnPlayerDisconnect -= HandlePlayerDisconnect;

        }

        if (IsClient && !IsServer)
        {

            NetworkManager.Singleton.OnClientStopped -= HandleDisconnect;

        }

    }

    private void HandlePlayerConnect(string authId, ulong cliendId)
    {

        Debug.Log(authId);
        StartCoroutine(GameStartCo());

    }

    [ServerRpc]
    private void SetTextServerRPC(string text)
    {

        SetTextClientRPC(text);

    }

    [ClientRpc]
    private void SetTextClientRPC(string text) 
    {
    
        loadingText.text = text;

    }

    private IEnumerator GameStartCo()
    {

        yield return new WaitForSeconds(1.5f);

        SetTextServerRPC("게임 시작");
        yield return new WaitForSeconds(3f);

        for(int i = 0; i < 3; i++)
        {

            SetTextServerRPC($"{3 - i}.....");
            yield return new WaitForSeconds(1);

        }

        NetworkManager.Singleton.SceneManager.LoadScene(SceneList.GameScene, LoadSceneMode.Single);

    }

}
