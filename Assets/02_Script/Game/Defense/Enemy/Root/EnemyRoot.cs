using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(FeedbackPlayer))]
public class EnemyRoot : NetworkBehaviour
{

    [field:SerializeField] public float maxHP { get; private set; }
    [SerializeField] protected float moveSpeed;

    protected Rigidbody2D rigid;
    protected FeedbackPlayer feedbackPlayer;
    protected float hp;
    protected string prefabKey;

    public float MoveValue { get; protected set; }

    protected virtual void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        feedbackPlayer = GetComponent<FeedbackPlayer>();
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
        feedbackPlayer.PlayFeedbackServerRPC(damage);

        if (hp <= 0)
        {

            DestroyObjectServerRPC();

            if (OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {

                FindObjectOfType<PlayerMoney>()?.AddMoney((int)maxHP);


                if (Random.value < 0.3f)
                {

                    DefenseManager.Instance.SpawnEnemyServerRPC(prefabKey, OwnerClientId);

                }

            }

        }

    }

    [ClientRpc]
    public void SetDirAndPosClientRPC(Vector2 ownerPos, Vector2 otherPos, ulong clientId, ulong ownerId, string key)
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

        prefabKey = key;

    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyObjectServerRPC()
    {

        Destroy(gameObject);

    }

}
