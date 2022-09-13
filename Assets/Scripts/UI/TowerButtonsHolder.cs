using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButtonsHolder : MonoBehaviour
{
    [SerializeField]
    private List<TowerButton> _towerButtons;

    public void Init(Vector3 position, int towerId)
    {
        gameObject.SetActive(true);
        transform.position = position;
        int buttonIndex = 0;
        var towerParams = TowerManager.Instance.GetParams(towerId);
        if (TowerManager.Instance.IsUpgradable(towerId))
        {
            //Инитим кнопку Upgrade
            var upgradeButton = _towerButtons[buttonIndex];
            buttonIndex++;
            upgradeButton.InitForUpgrade("grade " + (towerParams.Grade + 1), towerParams.UpgradePrice, () =>
            {
                TowerManager.Instance.TryUpgradeTower(towerId);
                Hide();
            });
            
            //Логику кнопки Salvage
            var salvageButton = _towerButtons[buttonIndex];
            buttonIndex++;
            salvageButton.InitForSalvage(towerParams.SalvagePrice, () =>
            {
                TowerManager.Instance.SalvageTower(towerId);
                Hide();
            });
        }
        
        if (TowerManager.Instance.IsConvertable(towerId))
        {
            var prices = TowerManager.Instance.GetConvertPrices(towerId);
            foreach (var price in prices)
            {
                var convertButton = _towerButtons[buttonIndex];
                buttonIndex++;
                convertButton.InitForConvert(price.toGrade.ToString(), price.convertPrice, () =>
                {
                    TowerManager.Instance.TryChangeTowerType(towerId, price.toTowerType, price.toGrade);
                    Hide();
                });
            }
        }

        for (int i = 0; i < _towerButtons.Count; i++)
        {
            _towerButtons[i].gameObject.SetActive(i < buttonIndex);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
