using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ArrowTower : TargetRotateTower
{

    private ArrowTowerAnimator animator;
    private LineRenderer line;

    protected override void Awake()
    {

        base.Awake();

        animator = GetComponent<ArrowTowerAnimator>();
        line = GetComponentInChildren<LineRenderer>();

        animator.OnAttackAnimeEnd += HandleAttackAnimeEnd;

    }

    public override void OnDestroy()
    {

        base.OnDestroy();

        animator.OnAttackAnimeEnd -= HandleAttackAnimeEnd;

    }

    protected override void DoAttack()
    {

        if (!IsOwner) return;

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

    private void HandleAttackAnimeEnd()
    {

        isAttackCalled = false;
        if (target == null) return;

        line.enabled = true;
        line.SetPosition(0, line.transform.position);
        line.SetPosition(1, target.transform.position);

        if (IsOwner)
        {

            target.TakeDamage(levelData[curLv].attackPower);
            StartCoroutine(AttackDelayCo());

        }

        StartCoroutine(LineReleseCo());

    }

    protected override void Rotate()
    {

        if (target == null) return;

        Vector2 dir = transform.position - target.transform.position;
        transform.right = -dir.normalized;

    }

    private IEnumerator LineReleseCo()
    {

        yield return new WaitForSeconds(0.05f);
        line.enabled = false;

    }

}
