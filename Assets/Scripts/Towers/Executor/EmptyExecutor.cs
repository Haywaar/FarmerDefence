using UnityEngine;
using Zenject;

namespace Towers.Executor
{
    public class EmptyExecutor : Executor
    {
        public override void Execute(TowerParams towerParams, Vector3 shootPos)
        {
            //do nothing
        }

        public EmptyExecutor(int towerId, SignalBus signalBus) : base(towerId, signalBus)
        {
        }
    }
}
