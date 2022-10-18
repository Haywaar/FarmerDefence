using System.Collections.Generic;

namespace Enemy.Meta
{
    [System.Serializable]
    public class EnemyRecord
    {
        public EnemyType EnemyType;
        public List<EnemyParams> EnemyParams;
    }
}