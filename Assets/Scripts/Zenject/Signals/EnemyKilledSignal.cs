using Units;

namespace Zenject.Signals
{
    public class EnemyKilledSignal
    {
        public readonly AbstractEnemy Enemy;
        public readonly int RewardPrice;
        
        public EnemyKilledSignal(AbstractEnemy enemy, int rewardPrice)
        {
            Enemy = enemy;
            RewardPrice = rewardPrice;
        }
    }
}