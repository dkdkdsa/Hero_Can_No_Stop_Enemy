using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelData
{

    public float attackPower;
    public float attackRange;
    public float attackCoolDown;
    public int levelUpCost;

}

public abstract class TowerRoot : NetworkBehaviour
{

    [SerializeField] protected List<LevelData> levelData = new();

    protected int curLv;
    protected bool isAttackCoolDown;

    protected virtual void Update()
    {

        if (!IsOwner) return;

    }

    protected abstract void DoAttack();

    [ClientRpc]
    public void SetPosClientRPC(Vector2 pos, ulong clientId)
    {

        var cPos = clientId == NetworkManager.Singleton.LocalClientId ? pos : -pos;

        transform.position = cPos;

    }

    private IEnumerator AttackDelayCo()
    {

        

    }

}
