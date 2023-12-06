using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlipAndMoveEnemy : EnemyRoot
{

    private SpriteRenderer spriteRenderer;
    private readonly int HASH_SINE_SCALE_FREQUENCY = Shader.PropertyToID("_SineScaleFrequency");

    public override void OnNetworkSpawn()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetFloat(HASH_SINE_SCALE_FREQUENCY, moveSpeed * 2);

    }

    protected override void Update()
    {

        base.Update();

        spriteRenderer.flipX = rigid.velocity.x >= 0;

    }

}