using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUIController : MonoBehaviour
{

    [SerializeField] private LobbyPanel lobbyPanelPrefab;
    [SerializeField] private TMP_InputField lobbyNameField;
    [SerializeField] private Transform content;

    private void Awake()
    {

        Refresh();

    }

    public async void Refresh()
    {

        var childs = content.GetComponentsInChildren<LobbyPanel>();

        foreach(var item in childs)
        {

            Destroy(item.gameObject);

        }

        var lobbys = await AppController.Instance.GetLobbyList();

        foreach(var item in lobbys)
        {

            Instantiate(lobbyPanelPrefab, content).Set(item);

        }

    }

    public async void CreateLobby()
    {

        var result = await AppController.Instance.StartHostAsync(
            FirebaseManager.Instance.userData.userName,
            lobbyNameField.text);

        if (result)
        {

            NetworkManager.Singleton.SceneManager.LoadScene(SceneList.LobbyScene, LoadSceneMode.Single);

        }
        else
        {

            Debug.LogError("로비 생성중 에러");

        }

    }

    public void Save()
    {

        FirebaseManager.Instance.SaveUserData();

    }

}
