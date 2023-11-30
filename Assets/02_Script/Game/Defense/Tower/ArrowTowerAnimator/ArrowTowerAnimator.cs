using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTowerAnimator : MonoBehaviour
{

    private readonly int HASH_ATTACK;

    private Animator animator;

    private void Awake()
    {
        
        animator = GetComponent<Animator>();

    }

}
