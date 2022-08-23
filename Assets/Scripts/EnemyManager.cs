using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    //TODO врагов будет много - надо на это настроить отдельный конфиг
    //TODO и вообще логику спаунинга врагов надо прокачать
    [Header("Spawning logic")] [SerializeField]
    private AbstractEnemy _enemyPrefab;

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float curSpawnInterval = 1.0f;

    private List<AbstractEnemy> _enemies = new List<AbstractEnemy>();
    private ObjectPool<AbstractEnemy> _pool;

    private EnemyManager()
    {
    }

    public static EnemyManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("DuplicateDetected!");
            Destroy(gameObject);
        }
        
        _pool = new ObjectPool<AbstractEnemy>(OnCreatedPoolItem, OnTakeFromPool, OnRelease, OnEnemyDestroy, true, 500);
    }

    private void OnEnemyDestroy(AbstractEnemy enemy)
    {
        Debug.LogWarning("ON ENEMY DESTROY");
    }


    private void Start()
    {
        StartCoroutine(PeriodicallySpawnCoroutine());
    }

    private IEnumerator PeriodicallySpawnCoroutine()
    {
        while (Player.Instance.IsAlive())
        {
            SpawnPrefab();
            yield return new WaitForSeconds(curSpawnInterval);
        }
    }

    private void SpawnPrefab()
    {
        
        //var enemy = GameObject.Instantiate(_enemyPrefab, _spawnPoint.position, Quaternion.identity);

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
       // Destroy(enemy.gameObject);
       _pool.Release(enemy);
    }
    public void OnTakeFromPool(AbstractEnemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.Init();
    }
    
    public AbstractEnemy OnCreatedPoolItem()
    {
        var enemy = Instantiate(_enemyPrefab, _spawnPoint.position, Quaternion.identity);
        return enemy;
    }
    
    private void OnRelease(AbstractEnemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
}