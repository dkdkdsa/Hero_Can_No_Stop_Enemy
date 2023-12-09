using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private LobbyUIController lobbyUIController;

    public void Set(Lobby lobby)
    {

        this.lobby = lobby;

        lobbyNameText.text = lobby.Name;
        humanText.text = $"({lobby.Players.Count}/{lobby.MaxPlayers})";

        lobbyUIController = FindObjectOfType<LobbyUIController>();

    }

    public async void StartGame()
    {

        lobbyUIController.loadingPanel.SetActive(true);

        try
        {

            Lobby joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);

            await AppController.Instance.StartClientAsync(FirebaseManager.Instance.userData.userName, joiningLobby.Data["JoinCode"].Value);

        }
        catch(System.Exception e)
        {

            Debug.LogException(e);

        }

        await Task.Delay(3000);

        if (lobbyUIController == null) return;

        lobbyUIController.loadingPanel.SetActive(false);

    }

}
