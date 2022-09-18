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
    private TowerManager _towerManager;
    
    [Inject]
    private void Construct(Player player, TowerManager towerManager)
    {
        _player = player;
        _towerManager = towerManager;
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
        _towerManager.OnTowerClicked += TowerClicked;
    }

    private void TowerClicked(int towerId)
    {
        if (towerId == -1)
        {
            if (_towerButtonsHolder.IsActive())
                _towerButtonsHolder.Hide();
        }
        else if (_towerManager.IsUpgradable(towerId) || _towerManager.IsConvertable(towerId))
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