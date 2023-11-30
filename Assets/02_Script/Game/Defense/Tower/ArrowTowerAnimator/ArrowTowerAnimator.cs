using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ArrowTowerAnimator : MonoBehaviour
{

    private readonly int HASH_ATTACK = Animator.StringToHash("Attack");

    private Animator animator;

    public event Action OnAttackAnimeEnd;

    private void Awake()
    {
        
        animator = GetComponent<Animator>();

    }

    public void SetAttackAnime()
    {
        
        animator.SetTrigger(HASH_ATTACK);
        
    }

    public void AttackAnimeEndExecute()
    {

        OnAttackAnimeEnd?.Invoke();

    }

}
