using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed;

    private void Update()
    {

        var dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Q))
        {

            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            DefenseManager.Instance.SpawnTowerServerRPC("Debug", pos, NetworkManager.Singleton.LocalClientId);

        }

    }

}
