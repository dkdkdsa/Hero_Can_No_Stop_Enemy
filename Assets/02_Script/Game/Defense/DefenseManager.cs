using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefenseManager : NetworkBehaviour
{

    [SerializeField] private EnemyListSO enemyData;

    [Space]
    [Header("EnemyWave")]
    [SerializeField] private Transform ownerPoint;
    [SerializeField] private Transform otherPoint;
    [Space]
    [SerializeField] private TowerListSO towerData;

    private Dictionary<ulong, List<EnemyRoot>> clientEnemyDic = new();
    private float defTime;

    public static DefenseManager Instance;

    private void Awake()
    {

        Instance = this;

    }

    public override void OnNetworkSpawn()
    {

        if (IsServer)
        {

            StartCoroutine(EnemySpawnCo());

        }

    }

    private void Update()
    {

        defTime += Time.deltaTime;

    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnTowerServerRPC(string towerPrefab, Vector2 originPos, ulong clientId)
    {

        var prefab = towerData.lists.Find(x => x.key == towerPrefab);
        var tower = Instantiate(prefab.obj);

        tower.SpawnWithOwnership(clientId, true);

        tower.GetComponent<TowerRoot>().SetPosClientRPC(originPos, clientId);

    }

    [ServerRpc(RequireOwnership = false)]
    public void GameDieServerRPC(ulong dieClient)
    {

        PlayerPrefs.SetInt("DieClient", (int)dieClient);
        NetworkManager.SceneManager.LoadScene(SceneList.ResultScene, LoadSceneMode.Single);

    }


    /// <summary>
    /// 적 소환
    /// </summary>
    /// <param name="enemyPrefab">키</param>
    /// <param name="clientId">소환을 요청한 클라이언트</param>
    [ServerRpc(RequireOwnership = false)]
    public void SpawnEnemyServerRPC(string enemyPrefab, ulong clientId)
    {

        var prefab = enemyData.list.Find(x => x.key == enemyPrefab);
        var netEnemy = Instantiate(prefab.netObj);

        var clients = NetworkManager.Singleton.ConnectedClientsIds;

        ulong ownerId = 0;

        foreach (var client in clients) 
        {

            if (client == clientId) continue;
            ownerId = client;

        }

        netEnemy.SpawnWithOwnership(ownerId, true);

        netEnemy.GetComponent<EnemyRoot>().SetDirAndPosClientRPC(
            ownerPoint.position, 
            otherPoint.position, 
            clientId, ownerId, enemyPrefab);

    }

    public void AddEnemy(EnemyRoot netObj, ulong ownerId)
    {

        if (!clientEnemyDic.ContainsKey(ownerId))
        {

            clientEnemyDic.Add(ownerId, new());

        }

        clientEnemyDic[ownerId].Add(netObj);

    }

    public List<EnemyRoot> GetEnemys(ulong clientId)
    {

        if (!clientEnemyDic.ContainsKey(clientId))
        {

            clientEnemyDic.Add(clientId, new());

        }

        return clientEnemyDic[clientId];

    }

    public void RemoveEnemy(EnemyRoot enemy, ulong clientId)
    {

        clientEnemyDic[clientId].Remove(enemy);

    }

    private IEnumerator EnemySpawnCo()
    {

        var clients = NetworkManager.Singleton.ConnectedClientsIds;

        yield return new WaitForSeconds(3f);

        Queue<string> spawnQ = new();

        while (true)
        {

            foreach(var enemy in enemyData.list)
            {

                if(defTime >= enemy.minTime)
                {

                    int cnt = (int)((defTime - enemy.minTime) / enemy.weight);

                    cnt = Mathf.Clamp(cnt, 0, 10);

                    for(int i = 0; i < cnt; i++)
                    {

                        spawnQ.Enqueue(enemy.key);

                    }

                }

            }

            while(spawnQ.Count > 0)
            {

                var key = spawnQ.Dequeue();

                foreach (var id in clients)
                {

                    SpawnEnemyServerRPC(key, id);

                }

                yield return new WaitForSeconds(Random.Range(0.7f, 1.5f));

            }



        }

    }

}
