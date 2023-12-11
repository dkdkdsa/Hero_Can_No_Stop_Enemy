using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAndGetMoneyTower : GunTower
{

    private int attackCount;
    private PlayerMoney playerMoney;

    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {

            playerMoney = FindObjectOfType<PlayerMoney>();

        }

    }

    protected override void DoAttack()
    {

        if (IsOwner && target != null)
        {

            attackCount++;

            if (attackCount == 6)
            {

                playerMoney.AddMoney(30);
                FAED.TakePool<MoneyTextEffect>("MoneyText", transform.position + Vector3.up / 2).SetText(30);
                attackCount = 0;

            }

        }

        base.DoAttack();

    }

}
