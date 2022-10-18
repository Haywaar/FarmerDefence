
using System.Collections.Generic;
using Units;
using UnityEngine;
using DG.Tweening;
using Zenject;
using Zenject.Signals;

namespace Towers.Executor
{
    public class ShootingExecutor : Executor
    {
        private AbstractEnemy target;
        public override void Execute(TowerParams towerParams, Vector3 shootPos)
        {
            if(_targetList.Count == 0)
                return;
           
            if (!_targetList.Contains(target))
                target = _targetList[0];
            
            //TODO - move shots on pool
            var shot = GameObject.Instantiate(towerParams.ShootPrefab, shootPos, Quaternion.identity);
            var tween = shot.transform.DOMove(target.transform.position, 0.1f, false);
            tween.onComplete += () =>
            {
                var signal = new EnemyDamagedSignal(target.ID, towerParams.DamageValue);
                _signalBus.Fire(signal);
                
                GameObject.Destroy(shot);
            };
        }

        public ShootingExecutor(int towerId, SignalBus signalBus) : base(towerId, signalBus)
        {
        }
    }
}
