using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{

    [SerializeField] private ClientSingle clientPrefab;
    [SerializeField] private HostSingle hostPrefab;

    public static AppController Instance;

    private void Awake()
    {
        
        Instance = this;

    }

    private async void Start()
    {

        DontDestroyOnLoad(gameObject);

        await UnityServices.InitializeAsync();

        var state = await AuthenticationWrapper.DoAuth(3);

        if(state != AuthState.Authenticated)
        {

            Debug.LogError("인증 실패");
            return;

        }

        HostSingle host = Instantiate(hostPrefab, transform);
        host.CreateHost();

        ClientSingle client = Instantiate(clientPrefab, transform);
        client.CreateClient();

        SceneManager.LoadScene(SceneList.MenuScene);

    }

}
