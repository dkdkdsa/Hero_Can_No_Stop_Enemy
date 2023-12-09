using FD.Dev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GunTower : TowerRoot
{

    [SerializeField] private List<Gun> gunByLevel;
    [SerializeField] private Transform gunPos;

    private SpriteRenderer spriteRenderer;
    private Gun currentGun;

    protected override void Awake()
    {
        
        base.Awake();

        currentGun = Instantiate(gunByLevel[CurLv], gunPos);
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentGun.transform.position = gunPos.position;
        OnLevelUpEvent += HandleLevelUp;

    }

    private void HandleLevelUp()
    {

        Destroy(currentGun.gameObject);
        currentGun = Instantiate(gunByLevel[CurLv], gunPos);
        currentGun.transform.position = gunPos.position;

    }

    protected override void Update()
    {

        base.Update();

        Rotate();

    }

    private void Rotate()
    {

        if (target == null) return;

        Vector2 dir = currentGun.transform.position - target.transform.position;
        currentGun.transform.right = -dir.normalized;

        spriteRenderer.flipX = dir.normalized.x > 0;
        currentGun.Flip(dir.normalized.x > 0);

    }

    protected override void DoAttack()
    {

        if(target != null)
        {

            FAED.TakePool<Bullet>("Bullet").Shoot(target.transform, levelData[CurLv].attackPower, currentGun.shootPos.position);
            FAED.TakePool<ParticleSystem>("ShootFX", currentGun.shootPos.position).Play();

            if (IsOwner)
            {

                StartCoroutine(AttackDelayCo());

            }

        }

        isAttackCalled = false;

    }

}
