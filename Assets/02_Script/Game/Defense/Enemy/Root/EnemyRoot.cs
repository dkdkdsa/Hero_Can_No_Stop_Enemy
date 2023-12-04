using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyRoot : NetworkBehaviour
{

    [field:SerializeField] public float maxHP { get; private set; }
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rigid;
    private float hp;

    public float MoveValue { get; protected set; }

    private void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        hp = maxHP;

    }

    protected virtual void Update()
    {

        MoveValue += Time.deltaTime * moveSpeed;

    }

    public override void OnDestroy()
    {
        
        base.OnDestroy();
        DefenseManager.Instance.RemoveEnemy(this, OwnerClientId);

    }

    public void SetDir(Vector2 dir)
    {

        rigid.velocity = dir * moveSpeed;

    }

    public void TakeDamage(float damage)
    {

        if (hp <= 0) return;

        hp -= damage;


        if (OwnerClientId == NetworkManager.Singleton.LocalClientId)
        {

            FindObjectOfType<PlayerMoney>().AddMoney((int)maxHP);

        }

        if (hp <= 0)
        {

            DestroyObjectServerRPC();

        }

    }

    [ClientRpc]
    public void SetDirAndPosClientRPC(Vector2 ownerPos, Vector2 otherPos, ulong clientId, ulong ownerId)
    {

        DefenseManager.Instance.AddEnemy(this, ownerId);

        if(NetworkManager.Singleton.LocalClientId == clientId)
        {

            transform.position = otherPos;
            SetDir(Vector2.down);

        }
        else
        {

            transform.position = ownerPos;
            SetDir(Vector2.up);

        }

    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyObjectServerRPC()
    {

        Destroy(gameObject);

    }

}
