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

    [Space]
    [Header("EnemyWave")]
    [SerializeField] private Transform ownerPoint;
    [SerializeField] private Transform otherPoint;

    private List<NetworkObject> enemyList = new();

    public List<TowerObj> towerList = new();
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

    public void AddEnemy(NetworkObject netObj)
    {

        enemyList.Add(netObj);

    }

    public List<NetworkObject> GetEnemys()
    {

        return enemyList;

    }

    private IEnumerator EnemySpawnCo()
    {

        var clients = NetworkManager.Singleton.ConnectedClientsIds;

        yield return new WaitForSeconds(3f);

        while (true)
        {

            foreach(var id in clients)
            {

                Debug.Log(id);
                SpawnEnemyServerRPC("Debug", id);

            }

            yield return new WaitForSeconds(1);

        }

    }

}
