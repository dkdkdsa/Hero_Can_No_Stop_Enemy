using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadEnd : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.CompareTag("Enemy"))
        {

            Destroy(collision.gameObject);

        }

    }
}
