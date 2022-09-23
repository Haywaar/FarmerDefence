using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _towerIcon;
    [SerializeField] private Text _priceText;

    private void Init()
    {
        _button.onClick.RemoveAllListeners();
        _priceText.text = String.Empty;
    }

    public void InitForUpgrade(Sprite towerTypeSprite, int price, UnityAction action)
    {
        Init();
        _priceText.text = price.ToString();
        _towerIcon.sprite = towerTypeSprite;
        _button.onClick.AddListener(action);
    }

    public void InitForSalvage(Sprite towerTypeSprite, int salvagePrice, UnityAction action)
    {
        Init();
        _towerIcon.sprite = towerTypeSprite;
        _priceText.text = "+" + salvagePrice;
        _button.onClick.AddListener(action);
    }

    public void InitForConvert(Sprite towerTypeSprite, int price, UnityAction action)
    {
        Init();
        _priceText.text = price.ToString();
        _towerIcon.sprite = towerTypeSprite;
        _button.onClick.AddListener(action);
    }
}