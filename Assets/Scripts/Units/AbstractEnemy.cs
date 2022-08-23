using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        [SerializeField] protected float _movementSpeed;
        [SerializeField] protected int _health;
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected int _damageValue;
        [SerializeField] protected int _rewardPrice;

        private bool _isDead = false;

        public bool IsDead => _isDead;

        private int _waypointId = 0;
        private List<Transform> _waypoints;
        public event Action<AbstractEnemy> OnEnemyDied;


        public int Health
        {
            get => _health;
            set
            {
                if (value == _health)
                    return;

                if (value <= 0)
                {
                    _health = 0;
                }

                if (value >= _maxHealth)
                {
                    _health = _maxHealth;
                }

                _health = value;
            }
        }

        public abstract void Attack();

        public virtual void Init()
        {
            _waypointId = 0;
            _health = _maxHealth;
            _isDead = false;
        }

        public virtual void Move()
        {
            var step = _movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _waypoints[_waypointId].position, step);

            if (Vector3.Distance(transform.position, _waypoints[_waypointId].position) < 0.02f)
            {
                if (_waypointId == _waypoints.Count - 1)
                {
                    Attack();
                    EnemyManager.Instance.Dispose(this);
                }
                else
                {
                    _waypointId++;
                }
            }
        }

        public virtual void OnDamageTaken(int damageValue)
        {
            Health -= damageValue;

            if (Health <= 0)
            {
                EnemyDead();
            }
        }

        public virtual void EnemyDead()
        {
            if (!_isDead)
            {
                _isDead = true;
                Player.Instance.OnMonsterDead(_rewardPrice);
                OnEnemyDied?.Invoke(this);
                EnemyManager.Instance.Dispose(this);
            }
        }

        private void Start()
        {
            //TODO - как стабилизировать?
            try
            {
                _waypoints = WaypointManager.Instance.GetPositions();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool CanMove()
        {
            return !_isDead && Player.Instance.IsAlive();
        }

    }
}