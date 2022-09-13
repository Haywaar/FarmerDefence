
using System.Collections.Generic;
using Units;
using UnityEngine;
using DG.Tweening;

namespace Towers.Executor
{
    public class ShootingExecutor : Executor
    {
        public override void Execute(TowerParams towerParams, Vector3 shootPos)
        {
            if(_targetList.Count == 0)
                return;
            
            var target = _targetList[0];
            //TODO - make pool
            var shot = GameObject.Instantiate(towerParams.ShootPrefab, shootPos, Quaternion.identity);
            var tween = shot.transform.DOMove(target.transform.position, 0.1f, false);
            tween.onComplete += () =>
            {
                target.OnDamageTaken(towerParams.DamageValue);
                GameObject.Destroy(shot);
            };
        }
    }
}
