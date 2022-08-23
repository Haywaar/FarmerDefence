using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _towerIcon;
    [SerializeField] private Text _priceText;

    //TODO - временно, удоли потом
    [SerializeField] private Text _towerTypeText;

    private void Init()
    {
        _button.onClick.RemoveAllListeners();
        _priceText.text = String.Empty;
        _towerTypeText.text = String.Empty;
    }

    public void InitForUpgrade(string towerTypeText, int price, UnityAction action)
    {
        Init();
        _priceText.text = price.ToString();
        _towerTypeText.text = towerTypeText;
        _button.onClick.AddListener(action);
    }

    public void InitForSalvage(int salvagePrice, UnityAction action)
    {
        Init();
        _priceText.text = "+" + salvagePrice;
        _towerTypeText.text = "SALVAGE!";
        _button.onClick.AddListener(action);
    }

    public void InitForConvert(string towerTypeText, int price, UnityAction action)
    {
        Init();
        _priceText.text = price.ToString();
        _towerTypeText.text = towerTypeText;
        _button.onClick.AddListener(action);
    }
}