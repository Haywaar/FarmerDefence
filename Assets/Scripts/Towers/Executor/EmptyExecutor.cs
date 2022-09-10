
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Towers.Executor
{
    public class EmptyExecutor : Executor
    {
        public override void Execute(TowerParams towerParams, Vector3 shootPos)
        {
            //do nothing
        }
    }
}
