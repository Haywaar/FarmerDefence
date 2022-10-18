using System.Collections.Generic;
using Units;

namespace Zenject.Signals
{
    public class EnemyInRadiusSignal
    {
        public int TowerId;
        public List<AbstractEnemy> EnemiesInRadius;

        public EnemyInRadiusSignal(int towerId, List<AbstractEnemy> enemiesInRadius)
        {
            TowerId = towerId;
            EnemiesInRadius = enemiesInRadius;
        }
    }
}