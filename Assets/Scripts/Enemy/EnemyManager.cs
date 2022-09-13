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

    private List<AbstractEnemy> _enemies = new List<AbstractEnemy>();
    private ObjectPool<AbstractEnemy> _pool;

    private DiContainer _container;
    private SignalBus _signalBus;

    private MonoMemoryPool<AbstractEnemy> _monoPool;

    [Inject]
    private void Construct(DiContainer container, Player player, SignalBus signalBus)
    {
        _container = container;
        _signalBus = signalBus;
    }

    private void Awake()
    {
        //   _pool = new ObjectPool<AbstractEnemy>(OnCreatedPoolItem, OnTakeFromPool, OnRelease, OnEnemyDestroy, true, 500);
    }

    private void OnEnemyDestroy(AbstractEnemy enemy)
    {
        //TODO 
    }


    private void Start()
    {
        _signalBus.Subscribe<SpawnEnemySignal>(OnSignalSpawn);
    }

    private void OnSignalSpawn(SpawnEnemySignal signal)
    {
        var enemy = signal.Enemy;
        var go = Instantiate(enemy, _spawnPoint.position, Quaternion.identity);
        _container.Inject(go);
        //   enemy.transform.position = _spawnPoint.position;
        go.Init();
        _enemies.Add(go);
    }

    // private void SpawnPrefab()
    // {
    //     var enemy = _pool.Get();
    //     enemy.transform.position = _spawnPoint.position;
    //     _enemies.Add(enemy);
    // }

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
        //TODO - fix
        //    _pool.Release(enemy);
    }

    private void OnTakeFromPool(AbstractEnemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.Init();
    }

    private AbstractEnemy OnCreatedPoolItem()
    {
        //   var enemy = _container.InstantiatePrefabForComponent<AbstractEnemy>(_enemyPrefab, _spawnPoint);
        //   return enemy;
        return null;
    }

    private void OnRelease(AbstractEnemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
}