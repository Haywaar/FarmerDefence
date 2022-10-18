using System;
using UnityEngine;
using UniRx;
using Zenject;
using Zenject.Signals;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private int _money;

    public readonly IntReactiveProperty HealthProperty = new IntReactiveProperty();
    public readonly IntReactiveProperty MoneyProperty = new IntReactiveProperty();
    public Action PlayerDead;

    public int Money
    {
        get => _money;
        private set
        {
            if (_money == value)
                return;

            if (_money < 0)
            {
                _money = 0;
            }
            else
            {
                _money = value;
            }

            MoneyProperty.Value = _money;
        }
    }

    public int Health
    {
        get => _health;
        private set
        {
            if (_health == value)
                return;

            if (value < 0)
            {
                _health = 0;
            }
            else if (value > _maxHealth)
            {
                _health = _maxHealth;
            }
            else
            {
                _health = value;
            }

            HealthProperty.Value = _health;
        }
    }

    private SignalBus _signalBus;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;

        _signalBus.Subscribe<EnemyKilledSignal>(OnEnemyKilled);
        _signalBus.Subscribe<ChangeMoneySignal>(ChangeMoney);
    }

    private void ChangeMoney(ChangeMoneySignal signal)
    {
        Money += signal.MoneyDelta;

        if (signal.MoneyDelta > 0)
        {
            // positive effect/ui logic
        }
        else if (signal.MoneyDelta < 0)
        {
            // negative/spending effect logic
        }
    }

    public void DamagePlayer(int value)
    {
        Health -= value;
        if (Health <= 0)
        {
            PlayerDead?.Invoke();
        }
    }

    public bool IsAlive()
    {
        return Health > 0;
    }

    private void OnEnemyKilled(EnemyKilledSignal signal)
    {
        Money += signal.RewardPrice;
    }
}