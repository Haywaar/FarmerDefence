using UnityEngine;

namespace Enemy.EnemySpawners
{
    public abstract class AbstractEnemySpawner : MonoBehaviour
    {
        protected bool IsSpawning = false;
        public abstract void StartSpawning();
        public abstract void StopSpawning();
    }
}