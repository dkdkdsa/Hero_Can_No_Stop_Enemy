using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBlinkFeedback : Feedback
{

    private readonly int HASH_ADD_COLOR_FADE = Shader.PropertyToID("_AddColorFade");
    private SpriteRenderer spriteRenderer;
    private float blinkTime;
    private bool isBlink = false;

    public HitBlinkFeedback(FeedbackPlayer player, float blinkTime = 0.07f) : base(player)
    {

        spriteRenderer = player.GetComponent<SpriteRenderer>();
        this.blinkTime = blinkTime;

    }

    public override void Play(float damage)
    {

        if (isBlink) return;

        player.StartCoroutine(BlinkCo());

    }

    private IEnumerator BlinkCo()
    {

        isBlink = true;

        spriteRenderer.material.SetFloat(HASH_ADD_COLOR_FADE, 1);

        yield return new WaitForSeconds(blinkTime);

        spriteRenderer.material.SetFloat(HASH_ADD_COLOR_FADE, 0);

        isBlink = false;

    }

}
