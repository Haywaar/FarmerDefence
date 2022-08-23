using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;
using UnityEngine.UI;

//TODO подруби таки Zenject
public class MainScreen : MonoBehaviour
{
    [SerializeField] private Text _healthValueText;
    [SerializeField] private Text _enemiesKilledText;
    [SerializeField] private Text _moneyText;
    [SerializeField] private TowerButtonsHolder _towerButtonsHolder;
    private const string healthLabelPrefix = "Health: ";
    private const string enemiesKilledPrefix = "Enemies killed: ";
    private const string moneyPrefix = "Money value: ";

    private void Start()
    {
        HealthChanged(Player.Instance.Health);
        MoneyChanged(Player.Instance.Health);
        _enemiesKilledText.text = enemiesKilledPrefix;

        Player.Instance.HealthChanged += HealthChanged;
        Player.Instance.PlayerDead += PlayerDead;
        Player.Instance.MoneyChanged += MoneyChanged;
        TowerManager.Instance.OnTowerClicked += TowerClicked;
    }

    private void TowerClicked(int towerId)
    {
        Debug.LogWarning("On Tower clicked!");
        if (TowerManager.Instance.IsUpgradable(towerId) || TowerManager.Instance.IsConvertable(towerId))
        {
            _towerButtonsHolder.Init(Input.mousePosition, towerId);
        }
    }

    private void PlayerDead()
    {
        DialogManager.ShowWindow<YouLoseDialog>();
    }

    private void HealthChanged(int value)
    {
        _healthValueText.text = healthLabelPrefix + Player.Instance.Health;
    }

    private void MoneyChanged(int value)
    {
        _moneyText.text = moneyPrefix + Player.Instance.Money;
    }
}