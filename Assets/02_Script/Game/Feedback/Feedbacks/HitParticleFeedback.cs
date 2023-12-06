using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleFeedback : Feedback
{
    public HitParticleFeedback(FeedbackPlayer player) : base(player)
    {
    }

    public override void Play(float damage)
    {

        FAED.TakePool<ParticleSystem>("HitParticle", transform.position).Play();

    }
}
