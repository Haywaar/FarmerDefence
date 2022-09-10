using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public enum SpawningLogic
{
    Periodic = 0,
    Curve = 1,
    FileData = 2,
}

public class EnemyManager : MonoBehaviour
{
    //TODO врагов будет много - надо на это настроить отдельный конфиг
    //TODO и вообще логику спаунинга врагов надо прокачать
    [Header("Spawning logic")] [SerializeField]
    private AbstractEnemy _enemyPrefab;

    [SerializeField] private SpawningLogic _spawningLogic;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float curSpawnInterval = 1.0f;

    private List<AbstractEnemy> _enemies = new List<AbstractEnemy>();
    private ObjectPool<AbstractEnemy> _pool;
    private DiContainer _container;
    private Player _player;

    [Inject]
    private void Construct(DiContainer container, Player player)
    {
        _container = container;
        _player = player;
    }

    private void Awake()
    {
        _pool = new ObjectPool<AbstractEnemy>(OnCreatedPoolItem, OnTakeFromPool, OnRelease, OnEnemyDestroy, true, 500);
    }

    private void OnEnemyDestroy(AbstractEnemy enemy)
    {
        //TODO 
    }


    private void Start()
    {
        StartCoroutine(PeriodicallySpawnCoroutine());
    }

    private IEnumerator PeriodicallySpawnCoroutine()
    {
        while (_player.IsAlive())
        {
            SpawnPrefab();
            yield return new WaitForSeconds(curSpawnInterval);
        }
    }

    private void SpawnPrefab()
    {
        var enemy = _pool.Get();
        enemy.transform.position = _spawnPoint.position;
        _enemies.Add(enemy);
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
        _pool.Release(enemy);
    }

    private void OnTakeFromPool(AbstractEnemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.Init();
    }

    private AbstractEnemy OnCreatedPoolItem()
    {
        var enemy = _container.InstantiatePrefabForComponent<AbstractEnemy>(_enemyPrefab, _spawnPoint);
        return enemy;
    }

    private void OnRelease(AbstractEnemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
}
