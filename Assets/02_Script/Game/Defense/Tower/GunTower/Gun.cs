using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [field:SerializeField] public Transform shootPos { get; private set; }

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void Flip(bool flip)
    {

        spriteRenderer.flipY = flip;

    }

}
