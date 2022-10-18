using Towers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

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
        _player.HealthProperty.Subscribe(HealthChanged);
        _player.MoneyProperty.Subscribe(MoneyChanged);

        HealthChanged(_player.Health);
        MoneyChanged(_player.Money);
        _enemiesKilledText.text = enemiesKilledPrefix;

        //TODO - on signal bus
        _player.PlayerDead += PlayerDead;
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
        _healthValueText.text = healthLabelPrefix + value;
    }

    private void MoneyChanged(int value)
    {
        _moneyText.text = moneyPrefix + value;
    }
}