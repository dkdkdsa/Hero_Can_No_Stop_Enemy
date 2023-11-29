using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TowerRoot : NetworkBehaviour
{

    [ClientRpc]
    public void SetPosClientRPC(Vector2 pos, ulong clientId)
    {

        var cPos = clientId == NetworkManager.Singleton.LocalClientId ? pos : -pos;

        transform.position = cPos;

    }

}
