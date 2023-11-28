using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class HostGameManager : IDisposable
{

    private Allocation allocation;
    private string joinCode;
    private string lobbyId;
    private const int MAX_CONNECTIONS = 2;

    public NetworkServer NetServer { get; private set; }

    public event Action<string, ulong> OnPlayerConnect;
    public event Action<string, ulong> OnPlayerDisconnect;

    public async Task<bool> StartHostAsync(string lobbyName, UserData userData)
    {

        try
        {

            allocation = await Relay.Instance.CreateAllocationAsync(MAX_CONNECTIONS);
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);

            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            var relayServerData = new RelayServerData(allocation, "dtls");
            transport.SetRelayServerData(relayServerData);

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            lobbyOptions.Data = new Dictionary<string, DataObject>()
            {

                {

                    "JoinCode", new DataObject(visibility: DataObject.VisibilityOptions.Member, value: joinCode)

                }

            };

            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, MAX_CONNECTIONS, lobbyOptions);
            lobbyId = lobby.Id;

            HostSingle.Instance.StartCoroutine(HeartbeatLobby(10));

            NetServer = new NetworkServer(NetworkManager.Singleton);
            NetServer.OnClientJoinEvent += HandleClientJoin;
            NetServer.OnClientLeftEvent += HandleClientLeft;

            string userJson = JsonUtility.ToJson(userData);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(userJson);
            NetworkManager.Singleton.StartHost();

            return true;

        }
        catch(Exception ex)
        {

            Debug.LogException(ex);
            return false;

        }

    }

    private void HandleClientJoin(string authId, ulong clientId)
    {

        OnPlayerConnect?.Invoke(authId, clientId);

    }

    private void HandleClientLeft(string authId, ulong clientId)
    {

        OnPlayerDisconnect?.Invoke(authId, clientId);

    }
    public void Dispose()
    {

        ShutdownAsync();

    }

    public async void ShutdownAsync()
    {

        if (!string.IsNullOrEmpty(lobbyId))
        {
            if (HostSingle.Instance != null)
            {
                HostSingle.Instance.StopCoroutine(nameof(HeartbeatLobby));
            }

            try
            {
                await Lobbies.Instance.DeleteLobbyAsync(lobbyId);
            }
            catch (LobbyServiceException ex)
            {
                Debug.LogError(ex);
            }
        }

        NetServer.OnClientLeftEvent -= HandleClientLeft;
        NetServer.OnClientJoinEvent -= HandleClientJoin;
        lobbyId = string.Empty;
        NetServer?.Dispose();

    }

    private IEnumerator HeartbeatLobby(float time)
    {

        var timer = new WaitForSecondsRealtime(time);

        while (true)
        {

            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);

            yield return timer;

        }

    }

}
