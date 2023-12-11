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

    [Header("보상")]
    [SerializeField] private GameObject rewardBtn;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text loginCountText;

    private bool isRefreshCoolDown = false;

    public GameObject loadingPanel;

    private void Awake()
    {

        Refresh();
        CheckReword();

    }

    public async void Refresh()
    {

        if (isRefreshCoolDown) return; 
        isRefreshCoolDown = true;

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

        StartCoroutine(RefreshCoolDown());

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

    private void Update()
    {
        
        CheckReword();

    }

    private void CheckReword()
    {

        if (FirebaseManager.Instance.IsContinuousLogIn && FirebaseManager.Instance.userData.isRewardGet == false)
        {

            rewardBtn.SetActive(true);

        }

    }

    public async void Save()
    {

        await FirebaseManager.Instance.SaveUserData();

    }

    public async void GetReward()
    {

        int reward = 100 * FirebaseManager.Instance.userData.loginCount;

        FirebaseManager.Instance.userData.coin += reward;
        FirebaseManager.Instance.userData.isRewardGet = true;

        rewardPanel.SetActive(true);
        loginCountText.text = $"연속로그인 {FirebaseManager.Instance.userData.loginCount}일차!";
        rewardText.text = $" {reward}코인 획득!";
        rewardBtn.SetActive(false);

        await FirebaseManager.Instance.SaveUserData();

    }

    private IEnumerator RefreshCoolDown()
    {

        yield return new WaitForSeconds(5f);
        isRefreshCoolDown = false;

    }

}
