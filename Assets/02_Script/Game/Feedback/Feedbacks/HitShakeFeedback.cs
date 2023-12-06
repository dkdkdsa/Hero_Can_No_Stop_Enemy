using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitShakeFeedback : Feedback
{

    private readonly int HASH_VIBRATE_FADE = Shader.PropertyToID("_VibrateFade");
    private SpriteRenderer spriteRenderer;
    private bool isShake;
    private float shakeTime;

    public HitShakeFeedback(FeedbackPlayer player, float shakeTime = 0.07f) : base(player)
    {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        this.shakeTime = shakeTime;

    }

    public override void Play(float damage)
    {

        if (isShake) return;

        player.StartCoroutine(ShakeCo());

    }

    private IEnumerator ShakeCo()
    {

        isShake = true;

        spriteRenderer.material.SetFloat(HASH_VIBRATE_FADE, 1);

        yield return new WaitForSeconds(shakeTime);

        spriteRenderer.material.SetFloat(HASH_VIBRATE_FADE, 0);

        isShake = false;

    }

}
