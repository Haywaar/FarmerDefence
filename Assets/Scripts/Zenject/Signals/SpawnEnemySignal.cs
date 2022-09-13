using Units;

namespace Zenject.Signals
{
    public class SpawnEnemySignal
    {
        public int Grade;
        public EnemyType EnemyType;
        
        public SpawnEnemySignal(int grade, EnemyType enemyType)
        {
            Grade = grade;
            EnemyType = enemyType;
        }
    }
}