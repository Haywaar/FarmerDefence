using Units;

namespace Zenject.Signals
{
    public class SpawnEnemySignal
    {
        public AbstractEnemy Enemy { get; }

        public SpawnEnemySignal(AbstractEnemy enemy)
        {
            Enemy = enemy;
        }
    }
}