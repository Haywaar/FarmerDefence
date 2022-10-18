using System;
using System.Collections.Generic;
using Towers.Meta;
using Units;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace Towers.Executor
{
    public abstract class Executor
    {
        protected List<AbstractEnemy> _targetList = new List<AbstractEnemy>();
        protected SignalBus _signalBus;
        protected int _towerId;

        public Executor(int towerId, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _towerId = towerId;
            
            _signalBus.Subscribe<EnemyInRadiusSignal>(SetTargetList);
        }

        public abstract void Execute(TowerParams towerParams, Vector3 shootPos);

        private void SetTargetList(EnemyInRadiusSignal signal)
        {
            if (_towerId == signal.TowerId)
            {
                _targetList = signal.EnemiesInRadius;
            }
        }
    }
}