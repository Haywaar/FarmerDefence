using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private Text _healthValueText;
    [SerializeField] private Text _enemiesKilledText;
    [SerializeField] private Text _moneyText;
    [SerializeField] private TowerButtonsHolder _towerButtonsHolder;
    private const string healthLabelPrefix = "Health: ";
    private const string enemiesKilledPrefix = "Enemies killed: ";
    private const string moneyPrefix = "Money value: ";

    private Player _player;
    
    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void Start()
    {
        HealthChanged(_player.Health);
        MoneyChanged(_player.Health);
        _enemiesKilledText.text = enemiesKilledPrefix;

        //TODO - on signal bus
        _player.HealthChanged += HealthChanged;
        _player.PlayerDead += PlayerDead;
        _player.MoneyChanged += MoneyChanged;
        TowerManager.Instance.OnTowerClicked += TowerClicked;
    }

    private void TowerClicked(int towerId)
    {
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
        _healthValueText.text = healthLabelPrefix + _player.Health;
    }

    private void MoneyChanged(int value)
    {
        _moneyText.text = moneyPrefix + _player.Money;
    }
}