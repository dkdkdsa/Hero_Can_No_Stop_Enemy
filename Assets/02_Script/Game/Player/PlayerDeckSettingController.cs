using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckSettingController : MonoBehaviour
{

    [SerializeField] private TowerListSO towerData;
    [SerializeField] private SpriteRenderer towerSpawnAreaSprite;
    [SerializeField] private AreaObject towerCreateArea;

    private List<TowerRoot> towers = new();
    private Rect rect => new Rect(transform.position.x - 1 / 2, transform.position.y - 1 / 2, 1, 1);
    private bool isTowerCreating;
    private bool createAble;
    private string towerKey;

    private void Update()
    {

        if (!isTowerCreating) return;

        ChackCreateAble();
        SetSpawnSpriteColor();

        towerSpawnAreaSprite.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
        {

            CreateTower();

        }

    }

    private void CreateTower()
    {

        towerSpawnAreaSprite.gameObject.SetActive(false);

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

            if (area.towerArea.ChackOverlaps(rect))
            {

                vel2 =false; 
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

}
