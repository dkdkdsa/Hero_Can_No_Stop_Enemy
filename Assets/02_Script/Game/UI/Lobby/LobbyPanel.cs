using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyPanel : MonoBehaviour
{

    [SerializeField] private TMP_Text lobbyNameText;
    [SerializeField] private TMP_Text humanText;

    private Lobby lobby;

    public void Set(Lobby lobby)
    {

        this.lobby = lobby;

        lobbyNameText.text = lobby.Name;
        humanText.text = $"({lobby.Players.Count}/{lobby.MaxPlayers})";

    }

    public async void StartGame()
    {

        try
        {

            Lobby joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);

            await AppController.Instance.StartClientAsync(FirebaseManager.Instance.userData.userName, joiningLobby.Data["JoinCode"].Value);

        }
        catch(System.Exception e)
        {

            Debug.LogException(e);
            return;

        }

    }

}
