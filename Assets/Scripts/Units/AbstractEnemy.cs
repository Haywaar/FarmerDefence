using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Units
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        [SerializeField] protected float _movementSpeed;
        [SerializeField] protected int _health;
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected int _damageValue;
        [SerializeField] protected int _rewardPrice;
        [SerializeField] protected EnemyType _enemyType;

        public EnemyType EnemyType => _enemyType;

        private bool _isDead = false;

        public bool IsDead => _isDead;

        private int _waypointId = 0;
        private List<Transform> _waypoints;
        private float _moveKoef = 1f;
        private const float MIN_SLOW_KOEF = .3f;
        public event Action<AbstractEnemy> OnEnemyDied;
        protected EnemyManager _enemyManager;
        protected Player _player;
        protected WaypointManager _waypointManager;

        [Inject]
        private void Construct(EnemyManager enemyManager, Player player, WaypointManager waypointManager)
        {
            _enemyManager = enemyManager;
            _player = player;
            _waypointManager = waypointManager;
        }

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

        public virtual void Reset()
        {
            _waypointId = 0;
            _health = _maxHealth;
            _isDead = false;
            _waypoints = _waypointManager.GetPositions();
        }

        public void Init(EnemyParams enemyParams, EnemyType enemyType)
        {
            _maxHealth = enemyParams.MaxHealth;
            _damageValue = enemyParams.DamageValue;
            _rewardPrice = enemyParams.RewardPrice;
            _movementSpeed = enemyParams.MovementSpeed;
            _enemyType = enemyType;
            //TODO - color
            
            Reset();
        }

        public virtual void Move()
        {
            var step = _movementSpeed * Time.deltaTime * _moveKoef;
            transform.position = Vector3.MoveTowards(transform.position, _waypoints[_waypointId].position, step);

            transform.LookAt(_waypoints[_waypointId].position);

            if (Vector3.Distance(transform.position, _waypoints[_waypointId].position) < 0.02f)
            {
                if (_waypointId == _waypoints.Count - 1)
                {
                    Attack();
                    _enemyManager.Dispose(this);
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
                //TODO - On signal BUS
                _player.OnMonsterDead(_rewardPrice);
                OnEnemyDied?.Invoke(this);
                _enemyManager.Dispose(this);
            }
        }

      

        public bool CanMove()
        {
            return !_isDead && _player.IsAlive();
        }

        //TODO set stacking logic (2 slow towers at 1 enemy)
        public void SetSlowKoef(float slowKoef)
        {
            if (slowKoef < 0f)
            {
                slowKoef = MIN_SLOW_KOEF;
            }
            else if (slowKoef > 1f)
            {
                slowKoef = 1f;
            }
            else
            {
                _moveKoef = slowKoef;
            }
        }

    }
}