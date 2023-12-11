using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadEnd : MonoBehaviour
{

    [SerializeField] private Slider hpSlider;

    private float hp = 100;

    private void Awake()
    {
        
        hpSlider.maxValue = hp;
        hpSlider.value = hp;

    }

    private void Update()
    {

        hpSlider.value = hp;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (hp <= 0) return;

        if (collision.transform.CompareTag("Enemy"))
        {

            var enemy = collision.transform.GetComponent<EnemyRoot>();
            hp -= enemy.maxHP / 2;


            ulong owner = enemy.OwnerClientId;
            enemy.DestroyObjectServerRPC();

            if(hp <= 0)
            {

                DefenseManager.Instance.GameDieServerRPC(owner);

            }


        }

    }
}
