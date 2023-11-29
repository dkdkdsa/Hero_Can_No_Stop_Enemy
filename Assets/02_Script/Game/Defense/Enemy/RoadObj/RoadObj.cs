using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadObj : MonoBehaviour
{

    [SerializeField] private Vector2 dir;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.CompareTag("Enemy"))
        {

            collision.transform.GetComponent<EnemyRoot>().SetDir(dir);

        }

    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        
        var old = Gizmos.color;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(dir * 3));

        Gizmos.color = old;

    }

#endif

}
