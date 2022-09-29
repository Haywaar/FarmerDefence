using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using Zenject.Signals;

public enum SpawningLogic
{
    Periodic = 0,
    Curve = 1,
    FileData = 2,
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private EnemyData _enemyData;
    private List<AbstractEnemy> _enemies = new List<AbstractEnemy>();
    private ObjectPool<AbstractEnemy> _pool;

    private DiContainer _container;
    private SignalBus _signalBus;
    private EnemyFabric _enemyFabric;

    [Inject]
    private void Construct(DiContainer container, Player player, SignalBus signalBus)
    {
        _container = container;
        _signalBus = signalBus;
        _signalBus.Subscribe<SpawnEnemySignal>(OnSignalSpawn);
        _enemyFabric = new EnemyFabric(_enemyData, _container);
    }

    private void OnSignalSpawn(SpawnEnemySignal signal)
    {
        var go = _enemyFabric.CreateEnemy(signal.Grade, signal.EnemyType);
        go.transform.position = _spawnPoint.transform.position;
        _enemies.Add(go);
    }

    private void Update()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.CanMove())
            {
                enemy.Move();
            }
        }
    }

    public void Dispose(AbstractEnemy enemy)
    {
        _enemies.Remove(enemy);
        _enemyFabric.DisposeEnemy(enemy);
    }
}