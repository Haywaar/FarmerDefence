using Towers.Executor;
using Towers.Meta;
using UnityEngine;
using Zenject;

public class SlowExecutor : Executor
{
    public override void Execute(TowerParams towerParams, Vector3 shootPos)
    {
        foreach (var enemy in _targetList)
        {
            enemy.SetSlowKoef(0.35f);
        }
    }

    public SlowExecutor(int towerId, SignalBus signalBus) : base(towerId, signalBus)
    {
    }
}