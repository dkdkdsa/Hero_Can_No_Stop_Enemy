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

    public List<TowerObj> towerList = new();
    public static DefenseManager Instance;

    private void Awake()
    {

        Instance = this;

    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnTowerServerRPC(string towerPrefab, Vector2 originPos, ulong clientId)
    {

        var prefab = towerList.Find(x => x.key == towerPrefab);
        var tower = Instantiate(prefab.obj);

        tower.SpawnWithOwnership(clientId);

        tower.GetComponent<TowerRoot>().SetPosClientRPC(originPos, clientId);

    }


}
