using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ArrowTower : TargetRotateTower
{

    private ArrowTowerAnimator animator;

    private void Awake()
    {
        
        animator = GetComponent<ArrowTowerAnimator>();

    }

    protected override void DoAttack()
    {

        SetAnimationServerRPC();

    }

    [ServerRpc]
    private void SetAnimationServerRPC()
    {

        SetAnimationClientRPC();

    }

    [ClientRpc]
    private void SetAnimationClientRPC()
    {

        animator.SetAttackAnime();

    }

    protected override void Rotate()
    {

        if (target == null) return;

        Vector2 dir = transform.position - target.transform.position;
        transform.right = -dir.normalized;

    }

}
