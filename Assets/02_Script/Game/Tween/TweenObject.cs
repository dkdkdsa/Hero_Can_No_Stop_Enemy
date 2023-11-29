using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{

    Move,
    Rotate

}

public enum TweenType
{

    None,
    Local

}

[System.Serializable]
public class TweenEvent
{

    public ActionType actionType; 
    public TweenType tweenType;
    public Vector3 endValue;
    public float duration;
    public Ease ease;

}

public class TweenObject : MonoBehaviour
{

    [SerializeField] private List<TweenEvent> events;

    private bool isRunning;

    private void ExecuteTween(TweenEvent evt)
    {

        if(evt.actionType == ActionType.Move)
        {

            Sequence seq = DOTween.Sequence();
            isRunning = true;

            if(evt.tweenType == TweenType.None)
            {

                seq.Append(transform.DOMove(evt.endValue, evt.duration).SetEase(evt.ease));

            }
            else
            {

                seq.Append(transform.DOLocalMove(evt.endValue, evt.duration).SetEase(evt.ease));

            }

            seq.AppendCallback(() =>
            {

                isRunning = false;

            });

        }
        else
        {

            Sequence seq = DOTween.Sequence();
            isRunning = true;

            if (evt.tweenType == TweenType.None)
            {

                seq.Append(transform.DORotate(evt.endValue, evt.duration).SetEase(evt.ease));

            }
            else
            {

                seq.Append(transform.DOLocalRotate(evt.endValue, evt.duration).SetEase(evt.ease));

            }

            seq.AppendCallback(() =>
            {

                isRunning = false;

            });

        }

    }

    public void RunTween(int num)
    {

        if (isRunning) return;

        ExecuteTween(events[num]);

    }

}
