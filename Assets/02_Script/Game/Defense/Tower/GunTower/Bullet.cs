using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private bool isSetting = false;
    private Transform target;
    private Vector2 origin;
    private float damage;
    private float per;

    private void Update()
    {

        if (!isSetting) return;

        if(target == null)
        {

            Disable();
            return;

        }

        per += Time.deltaTime * 4;
        transform.position = Vector2.Lerp(origin, target.position, per);

    }

    public void Shoot(Transform target, float damage, Vector2 origin)
    {

        isSetting = true;
        this.target = target;
        this.damage = damage;
        this.origin = origin;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {

            var enemy = collision.GetComponent<EnemyRoot>();
            
            if(enemy.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {

                enemy.TakeDamage(damage);

            }

            Disable();

        }

    }

    public void Disable()
    {

        isSetting = false;
        per = 0;
        FAED.InsertPool(gameObject);

    }

}
