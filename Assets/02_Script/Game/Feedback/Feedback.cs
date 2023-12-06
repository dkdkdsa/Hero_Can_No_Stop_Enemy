using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Feedback
{

    protected Transform transform;
    protected GameObject gameObject;
    protected FeedbackPlayer player;

    public Feedback(FeedbackPlayer player)
    {

        transform = player.transform;
        gameObject = player.gameObject;
        this.player = player;

    }

    public abstract void Play(float damage);

}
