using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlipAndMoveEnemy : EnemyRoot
{

    private SpriteRenderer spriteRenderer;
    private readonly int HASH_SINE_SCALE_FREQUENCY = Shader.PropertyToID("_SineScaleFrequency");

    protected override void Awake()
    {

        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetFloat(HASH_SINE_SCALE_FREQUENCY, moveSpeed * 2);

    }

    protected override void Update()
    {

        base.Update();

        if(spriteRenderer == null)
        {

            spriteRenderer = GetComponent<SpriteRenderer>();

        }

        if(rigid.velocity.y != 0)
        {

            spriteRenderer.flipX = rigid.velocity.y <= 0;

        }
        else
        {

            spriteRenderer.flipX = rigid.velocity.x >= 0;

        }


    }

}