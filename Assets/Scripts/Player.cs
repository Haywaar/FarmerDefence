using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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

    //TODO Not sure that player should be responsible for this
    public void OnMonsterDead(int value)
    {
        Money += value;
    }

    //TODO add enum with reason of reduce
    public void ReduceMoney(int value)
    {
      Money -= value;
    }
    
    public void AddMoney(int value)
    {
        Money += value;
        //TODO - play happy sound effect
    }
}