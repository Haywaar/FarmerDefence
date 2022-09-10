using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private int _money;


    public Action<int> HealthChanged;
    public Action<int> MoneyChanged;
    public Action PlayerDead;
    
    //TODO надо это раскопать
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

            MoneyChanged?.Invoke(_money);
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

            HealthChanged?.Invoke(_health);
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

    //TODO мне не нравится, перенести в GameManager?
    public void OnMonsterDead(int value)
    {
        _money += value;
        MoneyChanged?.Invoke(_money);
    }

    //TODO потом enum с типом причины
    public void ReduceMoney(int value)
    {
        Money -= value;
        //TODO - play sad sound effect
    }
    
    public void AddMoney(int value)
    {
        Money += value;
        //TODO - play happy sound effect
    }
}