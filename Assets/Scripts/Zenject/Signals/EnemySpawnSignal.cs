namespace Zenject.Signals
{
    public class EnemySpawnSignal
    {
        public readonly int Grade;
        public readonly EnemyType EnemyType;
        
        public EnemySpawnSignal(int grade, EnemyType enemyType)
        {
            Grade = grade;
            EnemyType = enemyType;
        }
    }
}