using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerDeckSettingController : MonoBehaviour
{

    [SerializeField] private TowerListSO towerData;
    [SerializeField] private SpriteRenderer towerSpawnAreaSprite;
    [SerializeField] private AreaObject towerCreateArea;

    private List<TowerRoot> towers = new();
    private Rect rect => new Rect(towerSpawnAreaSprite.transform.position.x - 1 / 2, towerSpawnAreaSprite.transform.position.y - 1 / 2, 1, 1);
    private bool isTowerCreating;
    private bool createAble;
    private string towerKey;

    private void Update()
    {

        if (!isTowerCreating) return;

        ChackCreateAble();
        SetSpawnSpriteColor();

        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        towerSpawnAreaSprite.transform.position = dir;


        if (Input.GetMouseButtonUp(0))
        {

            CreateTower();

        }

    }

    private void CreateTower()
    {

        towerSpawnAreaSprite.gameObject.SetActive(false);
        isTowerCreating = false;

        if (!createAble) return;
        createAble = false;

        DefenseManager.Instance.SpawnTowerServerRPC(
            towerKey,
            towerSpawnAreaSprite.transform.position,
            NetworkManager.Singleton.LocalClientId);

    }

    private void SetSpawnSpriteColor()
    {

        var color = createAble ? Color.green : Color.red;
        color.a = 0.45f;

        towerSpawnAreaSprite.color = color;

    }

    private void ChackCreateAble()
    {

        bool vel1 = towerCreateArea.ChackOverlaps(rect);
        bool vel2 = true;

        foreach(var area in towers)
        {

            Debug.Log(area);

            if (area.towerArea.ChackOverlaps(rect))
            {

                vel2 = false; 
                break;

            }

        }

        createAble = vel1 && vel2;

    }

    public void StartTowerCreate(string key)
    {

        towerSpawnAreaSprite.gameObject.SetActive(true);
        isTowerCreating = true;
        towerKey = key;

    }

    public void TowerAdd(TowerRoot tower)
    {

        towers.Add(tower);

    }

    public void TowerRemove(TowerRoot tower)
    {

        towers.Remove(tower);

    }

}
