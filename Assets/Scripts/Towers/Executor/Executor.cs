using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Towers.Executor
{
    public abstract class Executor
    {
        protected List<AbstractEnemy> _targetList = new List<AbstractEnemy>();
        public abstract void Execute(TowerParams towerParams, Vector3 shootPos);

        public void AddEnemy(AbstractEnemy enemy)
        {
            _targetList.Add(enemy);
        }

        public void RemoveEnemy(AbstractEnemy enemy)
        {
            if (_targetList.Contains(enemy))
            {
                _targetList.Remove(enemy);
                EnemyRemoved(enemy);
            }
        }

        protected virtual void EnemyRemoved(AbstractEnemy enemy)
        {
            // override me
        }
    }
}