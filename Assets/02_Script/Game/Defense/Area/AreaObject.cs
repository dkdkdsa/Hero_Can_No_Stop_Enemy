using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaObject : MonoBehaviour
{

    [SerializeField] private float width, height;

    private Rect rect;

    private void Update()
    {

        rect = new Rect(transform.position.x - width / 2, transform.position.y - height / 2, width, height);

    }

    public bool ChackOverlaps(Rect rect)
    {

        return this.rect.Overlaps(rect) || rect.Overlaps(this.rect);

    }

    public bool ChackContains(Vector2 point)
    {

        return rect.Contains(point);

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
