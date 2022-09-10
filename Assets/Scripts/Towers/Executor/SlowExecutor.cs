using System.Collections;
using System.Collections.Generic;
using Towers.Executor;
using Units;
using UnityEngine;

public class SlowExecutor : Executor
{
    public override void Execute(TowerParams towerParams, Vector3 shootPos)
    {
        foreach (var enemy in _targetList)
        {
            enemy.SetSlowKoef(0.35f);
        }
    }

    protected override void EnemyRemoved(AbstractEnemy enemy)
    {
        enemy.SetSlowKoef(1f);
    }
}
