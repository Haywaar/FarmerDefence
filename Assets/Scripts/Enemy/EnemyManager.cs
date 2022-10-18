using System.Collections.Generic;
using Enemy.Meta;
using Units;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using Zenject.Signals;

namespace Enemy
{
    /// <summary>
    /// Spawning and destroying logic of enemies
    /// Spawners can be different
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private EnemyData _enemyData;
        private readonly List<AbstractEnemy> _enemies = new List<AbstractEnemy>();

        public List<AbstractEnemy> Enemies => _enemies;

        private ObjectPool<AbstractEnemy> _pool;

        private static int _enemiesCount = 0;
    
        private DiContainer _container;
        private SignalBus _signalBus;
        private EnemyFabric _enemyFabric;
    
        [Inject]
        private void Construct(DiContainer container, Player player, SignalBus signalBus)
        {
            _container = container;
            _signalBus = signalBus;
            _signalBus.Subscribe<EnemySpawnSignal>(OnSignalSpawn);
            _enemyFabric = new EnemyFabric(_enemyData, _container);
        }

        private void OnSignalSpawn(EnemySpawnSignal signal)
        {
            var go = _enemyFabric.CreateEnemy(signal.Grade, signal.EnemyType);
            go.transform.position = _spawnPoint.transform.position;
            _enemies.Add(go);
        }

        public void Dispose(AbstractEnemy enemy)
        {
            _enemies.Remove(enemy);
            _enemyFabric.DisposeEnemy(enemy);
        }

        public static int GenerateEnemyId()
        {
            var enemyId = _enemiesCount;
            _enemiesCount++;
            return enemyId;
        }

        public Vector3 GetSpawnPointPosition()
        {
            return _spawnPoint.position;
        }
    }
}