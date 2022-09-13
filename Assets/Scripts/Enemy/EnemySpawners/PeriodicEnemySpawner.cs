using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace Enemy.EnemySpawners
{
    public class PeriodicEnemySpawner : AbstractEnemySpawner
    {
        [SerializeField]
        private List<PeriodicSpawnData> _enemiesConfig;

        protected SignalBus _signalBus;
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        //TODO 
        private void Awake()
        {
            StartSpawning();
        }

        public override void StartSpawning()
        {
            IsSpawning = true;
            foreach (var enemyData in _enemiesConfig)
            {
                StartCoroutine(PeriodicSpawnCoroutine(enemyData));
            }
        }

        public override void StopSpawning()
        {
            IsSpawning = false;
        }

        private IEnumerator PeriodicSpawnCoroutine(PeriodicSpawnData spawnData)
        {
            var cooldown = spawnData.StartCooldown;
            var time = 0.0f;
            while (IsSpawning)
            {
                Debug.LogWarning("spawning send! Cooldown " + cooldown);
                _signalBus.Fire(new SpawnEnemySignal(spawnData.EnemyPrefab));
                yield return new WaitForSeconds(cooldown);
                time += cooldown;
                cooldown = Mathf.Lerp(spawnData.StartCooldown, spawnData.MinCooldown,
                    Mathf.Min(time / spawnData.IncreaseCooldownTime, 1));
            }
            Debug.LogWarning("Spawning stopped");
        }
    }

    [System.Serializable]
    public struct PeriodicSpawnData
    {
        public AbstractEnemy EnemyPrefab;
        /// <summary>
        /// Cooldown between spawns of different type
        /// </summary>
        public float StartCooldown;
        /// <summary>
        /// Shortes cooldown that can ever be
        /// </summary>
        public float MinCooldown;
        /// <summary>
        /// Duration(sec) of changing StartCooldown to MinCooldown 
        /// </summary>
        public float IncreaseCooldownTime;
    }
}