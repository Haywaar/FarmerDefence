using System.Collections.Generic;
using Enemy;
using Enemy.Meta;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace Units
{
    //TODO - Refactor parameters, move it to submodule
    //TODO - Change abstract enemy on enemy, move diff behaviour in submodule
    public abstract class AbstractEnemy : MonoBehaviour
    {
        [SerializeField] protected float _movementSpeed;
        [SerializeField] protected int _health;
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected int _damageValue;
        [SerializeField] protected int _rewardPrice;
        [SerializeField] protected EnemyType _enemyType;

        [SerializeField] protected List<MeshRenderer> _colorableMeshes;

        public int ID { get; private set; }

        public EnemyType EnemyType => _enemyType;
        public bool IsDead { get; private set; }

        private int _waypointId = 0;
        private List<Transform> _waypoints;
        private float _moveKoef = 1f;
        private const float MIN_SLOW_KOEF = .3f;
        
        protected EnemyManager _enemyManager;
        protected Player _player;
        protected WaypointManager _waypointManager;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(EnemyManager enemyManager, Player player, WaypointManager waypointManager, SignalBus signalBus)
        {
            _enemyManager = enemyManager;
            _player = player;
            _waypointManager = waypointManager;
            _signalBus = signalBus;
            _signalBus.Subscribe<EnemyDamagedSignal>(OnDamageTaken);
        }

        private int Health
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
            IsDead = false;
            _waypoints = _waypointManager.GetPositions();
        }

        private void Awake()
        {
            ID = EnemyManager.GenerateEnemyId();
        }

        public void Init(EnemyParams enemyParams, EnemyType enemyType)
        {
            _maxHealth = enemyParams.MaxHealth;
            _damageValue = enemyParams.DamageValue;
            _rewardPrice = enemyParams.RewardPrice;
            _movementSpeed = enemyParams.MovementSpeed;
            _enemyType = enemyType;
            //TODO - color
            foreach (var mesh in _colorableMeshes)
            {
                mesh.material = enemyParams.Material;
            }
            gameObject.name = enemyType.ToString() + "_" + enemyParams.Grade;

            Reset();
        }

        public virtual void Move(Vector3 newPos)
        {
            transform.position = newPos;

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

        public Vector3 GetNextStepPosition()
        {
            var step = _movementSpeed * Time.deltaTime * _moveKoef;
            return Vector3.MoveTowards(transform.position, _waypoints[_waypointId].position, step);
        }

        public float GetDistanceToWaypoint()
        {
            return Vector3.Distance(transform.position, _waypoints[_waypointId].position);
        }

        public int GetWaypointId()
        {
            return _waypointId;
        }

        private void OnDamageTaken(EnemyDamagedSignal signal)
        {
            if (ID != signal.EnemyId)
            {
                return;
            }

            Health -= signal.DamageValue;

            if (Health <= 0)
            {
                EnemyDead();
            }
        }

        private void EnemyDead()
        {
            if (!IsDead)
            {
                IsDead = true;

                _signalBus.Fire(new EnemyKilledSignal(this, _rewardPrice));
                _enemyManager.Dispose(this);
            }
        }

        public bool CanMove()
        {
            return !IsDead && _player.IsAlive();
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