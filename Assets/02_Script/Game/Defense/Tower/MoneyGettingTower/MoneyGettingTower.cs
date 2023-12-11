using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyGettingTower : TowerRoot
{

    private PlayerMoney playerMoney;

    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {

            playerMoney = FindObjectOfType<PlayerMoney>();
            StartCoroutine(MoneyGetCo());

        }

    }

    protected override void Update()
    {
    }

    protected override void DoAttack()
    {

        if (IsOwner)
        {

            playerMoney.AddMoney((int)levelData[CurLv].attackPower);
            FAED.TakePool<MoneyTextEffect>("MoneyText", transform.position + Vector3.up / 4).SetText((int)levelData[CurLv].attackPower);

        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoneyGetCo()
    {

        while (true)
        {

            yield return new WaitForSeconds(levelData[CurLv].attackCoolDown);
            DoAttack();

        }

    }

}
