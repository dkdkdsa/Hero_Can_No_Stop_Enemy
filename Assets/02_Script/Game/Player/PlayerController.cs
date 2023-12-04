using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private AreaObject spawnAbleArea;
    [SerializeField] private AreaObject moveAbleArea;
    [Space]
    [SerializeField] private float moveSpeed;

    public bool MoveAble;

    private void Update()
    {

        Move();

    }

    private void Move()
    {

        if (!MoveAble) return;

        var dir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        dir.Normalize();

        var nextPos = transform.position + dir * Time.deltaTime * moveSpeed;

        if(moveAbleArea.ChackContains(nextPos))
        {

            transform.position = nextPos;

        }

    }


}
