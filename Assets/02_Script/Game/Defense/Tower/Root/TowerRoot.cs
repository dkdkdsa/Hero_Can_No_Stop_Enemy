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

public delegate void LevelUp();

[RequireComponent(typeof(AreaObject))]
public abstract class TowerRoot : NetworkBehaviour
{

    [SerializeField] protected List<LevelData> levelData = new();
    [field:SerializeField] public string TowerKey { get; protected set; } 

    private bool isSetTargetCalled;

    private AreaObject area;
    protected EnemyRoot target;
    protected bool isAttackCoolDown;
    protected bool isAttackCalled;

    public event LevelUp OnLevelUpEvent;


    public AreaObject TowerArea 
    {
        get 
        { 
            if(area == null)
            {

                area = GetComponent<AreaObject>();

            }

            return area;

        }

        set { area = value; }
    }
    public List<LevelData> LvDataList => levelData;
    public int CurLv { get; protected set; }

    protected virtual void Awake()
    {

        TowerArea = GetComponent<AreaObject>();

    }

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

            if(dist <= Mathf.Pow(levelData[CurLv].attackRange, 2) 
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

            if (dist <= Mathf.Pow(levelData[CurLv].attackRange, 2)
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

        if (idx >= list.Count) return;

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


        if(clientId == NetworkManager.Singleton.LocalClientId)
        {

            FindObjectOfType<PlayerDeckSettingController>().TowerAdd(this);

        }

    }

    protected IEnumerator AttackDelayCo()
    {

        isAttackCoolDown = true;
        yield return new WaitForSeconds(levelData[CurLv].attackCoolDown);
        isAttackCoolDown = false;

    }

    [ServerRpc]
    public void LevelUpServerRPC()
    {

        LevelUpClientRPC();

    }

    [ClientRpc]
    private void LevelUpClientRPC()
    {

        LevelUp();

    }

    private void LevelUp()
    {

        if(CurLv + 1 != levelData.Count)
        {

            CurLv++;
            OnLevelUpEvent?.Invoke();

        }

    }

    public override void OnDestroy()
    {

        base.OnDestroy();

        if (NetworkManager.Singleton == null) return;

        if(OwnerClientId == NetworkManager.Singleton.LocalClientId)
        {

            FindObjectOfType<PlayerDeckSettingController>().TowerRemove(this);

        }

    }

    [ServerRpc]
    public void DestroyTowerServerRPC()
    {

        Destroy(gameObject);

    }

    private void OnMouseDown()
    {

        if (IsOwner)
        {

            UpgradeUIController.Instance.gameObject.SetActive(true);
            UpgradeUIController.Instance.SetPanel(this);

        }

    }

}
