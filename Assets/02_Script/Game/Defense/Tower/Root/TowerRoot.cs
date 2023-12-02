using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
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
    protected EnemyRoot target;

    private bool isSetTargetCalled;
    protected bool isAttackCalled;

    protected virtual void Update()
    {

        if (!IsOwner) return;

        if (!isAttackCoolDown)
        {

            if(target == null && isSetTargetCalled == false)
            {

                isSetTargetCalled = true;
                SetTarget();

            }

            if(target != null && isAttackCalled == false)
            {

                isAttackCalled = true;
                AttackServerRPC();

            }

        }

        ChackTarget();

    }

    protected virtual void SetTarget()
    {

        var list = DefenseManager.Instance.GetEnemys(OwnerClientId);

        float maxWalkValue = float.MinValue;
        int idx = -1;

        for(int i = 0; i < list.Count; i++)
        {

            float dist = (transform.position - list[i].transform.position).sqrMagnitude;

            if(dist <= Mathf.Pow(levelData[curLv].attackRange, 2) 
                && list[i].MoveValue > maxWalkValue)
            {

                maxWalkValue = list[i].MoveValue;
                idx = i;

            }

        }

        SetTargetServerRPC(idx);

    }

    protected virtual void ChackTarget()
    {

        var list = DefenseManager.Instance.GetEnemys(OwnerClientId);

        float maxWalkValue = float.MinValue;
        int idx = -1;

        for (int i = 0; i < list.Count; i++)
        {

            float dist = (transform.position - list[i].transform.position).sqrMagnitude;

            if (dist <= Mathf.Pow(levelData[curLv].attackRange, 2)
                && list[i].MoveValue > maxWalkValue)
            {

                maxWalkValue = dist;
                idx = i;

            }

        }

        if(idx != -1)
        {

            if (target != list[idx])
            {

                isSetTargetCalled = true;
                SetTarget();

            }

        }

    }

    [ServerRpc]
    private void SetTargetServerRPC(int idx)
    {

        SetTargetClientRPC(idx);

    }

    [ClientRpc]
    private void SetTargetClientRPC(int idx)
    {

        if (idx == -1) return;

        var list = DefenseManager.Instance.GetEnemys(OwnerClientId);

        target = list[idx];
        isSetTargetCalled = false;

    }

    [ServerRpc]
    private void AttackServerRPC()
    {

        AttackClientRPC();

    }

    [ClientRpc]
    private void AttackClientRPC()
    {

        DoAttack();

    }

    protected abstract void DoAttack();

    [ClientRpc]
    public void SetPosClientRPC(Vector2 pos, ulong clientId)
    {

        var cPos = clientId == NetworkManager.Singleton.LocalClientId ? pos : -pos;

        transform.position = cPos;

    }

    protected IEnumerator AttackDelayCo()
    {

        isAttackCoolDown = true;
        yield return new WaitForSeconds(levelData[curLv].attackCoolDown);
        isAttackCoolDown = false;

    }

}
