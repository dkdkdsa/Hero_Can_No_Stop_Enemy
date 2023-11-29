using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public class TowerObj
{

    public string key;
    public NetworkObject obj;

}

public class DefenseManager : NetworkBehaviour
{

    [SerializeField] private EnemyListSO enemyData;

    [Header("EnemyWave")]
    [SerializeField] private Transform ownerPoint, otherPoint;

    private Dictionary<ulong, List<NetworkObject>> cliendIdToenemyListDic;

    public List<TowerObj> towerList = new();
    public static DefenseManager Instance;

    private void Awake()
    {

        Instance = this;

    }

    private void Start()
    {

        if (!IsServer) return;

        StartCoroutine(EnemySpawnCo());

    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnTowerServerRPC(string towerPrefab, Vector2 originPos, ulong clientId)
    {

        var prefab = towerList.Find(x => x.key == towerPrefab);
        var tower = Instantiate(prefab.obj);

        tower.SpawnWithOwnership(clientId);

        tower.GetComponent<TowerRoot>().SetPosClientRPC(originPos, clientId);

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

        netEnemy.SpawnWithOwnership(ownerId);

        netEnemy.GetComponent<EnemyRoot>().SetDirAndPosClientRPC(ownerPoint.position, otherPoint.position, clientId);

    }

    private IEnumerator EnemySpawnCo()
    {

        yield return null;

        var clients = NetworkManager.Singleton.ConnectedClientsIds;

        while (true)
        {

            foreach(var id in clients)
            {

                SpawnEnemyServerRPC("Debug", id);

            }

        }

    }

}
