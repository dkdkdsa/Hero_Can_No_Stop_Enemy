using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyRoot : NetworkBehaviour
{

    [SerializeField] private float maxHP;
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rigid;
    private float hp;

    private void Awake()
    {

        hp = maxHP;

    }

    public void SetDir(Vector2 dir)
    {

        rigid.velocity = dir * moveSpeed;

    }

    public void TakeDamage(float damage)
    {

        hp -= damage;

        if(hp <= 0)
        {

            Destroy(gameObject);

        }

    }

    [ClientRpc]
    public void SetDirAndPosClientRPC(Vector2 ownerPos, Vector2 otherPos, ulong clientId)
    {

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

}
