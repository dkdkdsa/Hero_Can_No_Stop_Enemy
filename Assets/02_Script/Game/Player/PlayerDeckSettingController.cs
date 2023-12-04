using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckSettingController : MonoBehaviour
{

    [SerializeField] private TowerListSO towerData;
    [SerializeField] private SpriteRenderer towerSpawnArea;
    [SerializeField] private AreaObject towerCreateArea;

    private bool isTowerCreating;
    private string towerKey;
    private List<TowerObj> towers;

    private void Update()
    {

        if (!isTowerCreating) return;

    }

    private void CreateTower()
    {



    }

    public void StartTowerCreate(string key)
    {

        isTowerCreating = true;
        towerKey = key;

    }

}
