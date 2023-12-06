using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FeedbackPlayer : NetworkBehaviour
{

    private HashSet<Feedback> feedbackContainer = new();

    private void Awake()
    {

        var hitBlink = new HitBlinkFeedback(this);
        var hitShake = new HitShakeFeedback(this);
        var hitParticle = new HitParticleFeedback(this);

        feedbackContainer.Add(hitBlink);
        feedbackContainer.Add(hitShake);
        feedbackContainer.Add(hitParticle);

    }

    [ServerRpc]
    public void PlayFeedbackServerRPC(float damage)
    {

        PlayFeedbackClientRPC(damage);

    }

    [ClientRpc]
    private void PlayFeedbackClientRPC(float damage)
    {

        foreach(var feedback in feedbackContainer)
        {

            feedback.Play(damage);

        }

    }

}
