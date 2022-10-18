
namespace Zenject.Signals
{
    public class EnemyDamagedSignal
    {
        public readonly int EnemyId;
        public readonly int DamageValue;
        
        public EnemyDamagedSignal(int enemyId, int damageValue)
        {
            EnemyId = enemyId;
            DamageValue = damageValue;
        }
    }
}
