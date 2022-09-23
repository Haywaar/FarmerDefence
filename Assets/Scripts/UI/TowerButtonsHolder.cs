using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TowerButtonsHolder : MonoBehaviour
{
    [SerializeField]
    private List<TowerButton> _towerButtons;

    private TowerManager _towerManager;

    [Inject]
    private void Construct(TowerManager towerManager)
    {
        _towerManager = towerManager;
    }

    public void Init(Vector3 position, int towerId)
    {
        gameObject.SetActive(true);
        transform.position = position;
        int buttonIndex = 0;
        var towerParams = _towerManager.GetParams(towerId);
        if (_towerManager.IsUpgradable(towerId))
        {
            //Инитим кнопку Upgrade
            var upgradeButton = _towerButtons[buttonIndex];
            buttonIndex++;
            upgradeButton.InitForUpgrade(towerParams.TowerIcon, towerParams.UpgradePrice, () =>
            {
                _towerManager.TryUpgradeTower(towerId);
                Hide();
            });
            
            //Логику кнопки Salvage
            var salvageButton = _towerButtons[buttonIndex];
            buttonIndex++;
            var emptyIcon = _towerManager.GetTypeSprite(TowerType.Empty);
            salvageButton.InitForSalvage(emptyIcon, towerParams.SalvagePrice, () =>
            {
                _towerManager.SalvageTower(towerId);
                Hide();
            });
        }
        
        if (_towerManager.IsConvertable(towerId))
        {
            var prices = _towerManager.GetConvertPrices(towerId);
            foreach (var price in prices)
            {
                var convertButton = _towerButtons[buttonIndex];
                buttonIndex++;
                var towerIcon = _towerManager.GetTypeSprite(price.toTowerType);
                convertButton.InitForConvert(towerIcon, price.convertPrice, () =>
                {
                    _towerManager.TryConvert(towerId, price.toTowerType, price.toGrade);
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

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
