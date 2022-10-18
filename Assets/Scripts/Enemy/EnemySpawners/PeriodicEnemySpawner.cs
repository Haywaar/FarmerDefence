using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using Cysharp.Threading.Tasks;
using Enemy.Meta;

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
               PeriodicSpawnTask(enemyData);
            }
        }

        public override void StopSpawning()
        {
            IsSpawning = false;
        }

        private async void PeriodicSpawnTask(PeriodicSpawnData spawnData)
        {
            var cooldown = spawnData.StartCooldown;
            var time = 0.0f;
            await UniTask.Delay((int)(spawnData.PrewarmDelay * 1000));
            while (IsSpawning)
            {
                _signalBus.Fire(new EnemySpawnSignal(spawnData.EnemyGrade, spawnData.EnemyType));
                await UniTask.Delay((int)(cooldown * 1000));
                time += cooldown;
                cooldown = Mathf.Lerp(spawnData.StartCooldown, spawnData.MinCooldown,
                    Mathf.Min(time / spawnData.IncreaseCooldownTime, 1));
            }
        }
    }

    [System.Serializable]
    public struct PeriodicSpawnData
    {
        public EnemyType EnemyType;
        public int EnemyGrade;

        /// <summary>
        /// Delay between start game and first spawn
        /// </summary>
        public float PrewarmDelay;
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