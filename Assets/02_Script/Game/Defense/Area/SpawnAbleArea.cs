using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAbleArea : MonoBehaviour
{

    [SerializeField] private float width, height;

    private Rect rect;

    private void Awake()
    {
        
        rect = new Rect(transform.position.x - width / 2, transform.position.y - height /2, width, height);

    }

    public bool ChackMakeAble(Rect rect)
    {

        return this.rect.Overlaps(rect);

    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        
        var old = Gizmos.color;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));

        Gizmos.color = old;

    }

#endif

}
