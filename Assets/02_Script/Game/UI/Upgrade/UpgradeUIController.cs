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
    private PlayerMoney money;

    public static UpgradeUIController Instance;

    private void Awake()
    {

        money = FindObjectOfType<PlayerMoney>();
        Instance = this;
        gameObject.SetActive(false);

    }

    public void SetPanel(TowerRoot tower)
    {

        this.tower = tower;
        InitData();

    }

    public void SellingTower()
    {

        tower.DestroyTowerServerRPC();
        ReleasePanel();

    }

    public void ReleasePanel()
    {

        tower = null;
        gameObject.SetActive(false);

    }

    public void LevelUpTower()
    {

        if(money.GetMoney() >= tower.LvDataList[tower.CurLv].levelUpCost)
        {

            money.SubtractMonmy(tower.LvDataList[tower.CurLv].levelUpCost);
            tower.LevelUpServerRPC();
            InitData();

        }

    }

    private void InitData()
    {

        var data = towerData.lists.Find(x => x.key == tower.TowerKey);
        towerIcon.sprite = data.sprite;
        towerText.text = data.towerName;
        towerLevelText.text = $"LV : {tower.CurLv + 1}";

        var str = tower.CurLv + 1 == tower.LvDataList.Count ? "MAX" : tower.LvDataList[tower.CurLv + 1].attackPower.ToString("0.##");
        attackText.text = $"{tower.LvDataList[tower.CurLv].attackPower} -> {str}";

        str = tower.CurLv + 1 == tower.LvDataList.Count ? "MAX" : tower.LvDataList[tower.CurLv + 1].attackRange.ToString("0.##");
        rangeText.text = $"{tower.LvDataList[tower.CurLv].attackRange} -> {str}";

        str = tower.CurLv + 1 == tower.LvDataList.Count ? "MAX" : tower.LvDataList[tower.CurLv + 1].attackCoolDown.ToString("0.##");
        coolDownText.text = $"{tower.LvDataList[tower.CurLv].attackCoolDown} -> {str}";

        str = tower.CurLv + 1 == tower.LvDataList.Count ? "MAX" : tower.LvDataList[tower.CurLv].levelUpCost.ToString() + "$";
        upgradeText.text = $"°­È­ {str}";

    }

}
