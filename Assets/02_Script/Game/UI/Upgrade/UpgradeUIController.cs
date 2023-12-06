using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIController : MonoBehaviour
{

    [SerializeField] private Image towerIcon;
    [SerializeField] private TMP_Text towerText;
    [SerializeField] private TMP_Text towerLevelText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text coolDownText;
    [SerializeField] private TMP_Text rangeText;
    [SerializeField] private TMP_Text upgradeText;
    [SerializeField] private TowerListSO towerData;

    private TowerRoot tower;

    public void SetPanel(TowerRoot tower)
    {

        this.tower = tower;
        InitData();

    }

    private void InitData()
    {

        var data = towerData.lists.Find(x => x.key == tower.TowerKey);
        towerIcon.sprite = data.sprite;
        towerText.text = data.towerName;
        towerLevelText.text = $"LV : {tower.CurLv + 1}";

        var str = tower.CurLv + 1 == tower.LvDataList.Count ? "MAX" : tower.LvDataList[tower.CurLv].attackPower.ToString("#.##");
        attackText.text = $"{tower.LvDataList[tower.CurLv].attackPower} -> {str}";

        str = tower.CurLv + 1 == tower.LvDataList.Count ? "MAX" : tower.LvDataList[tower.CurLv].attackRange.ToString("#.##");
        rangeText.text = $"{tower.LvDataList[tower.CurLv].attackRange} -> {str}";

        str = tower.CurLv + 1 == tower.LvDataList.Count ? "MAX" : tower.LvDataList[tower.CurLv].attackCoolDown.ToString("#.##");
        coolDownText.text = $"{tower.LvDataList[tower.CurLv].attackCoolDown} -> {str}";

    }

}
