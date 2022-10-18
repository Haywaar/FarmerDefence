using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy.Meta
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyDataScriptableObject", order = 1)]
    public class EnemyData : ScriptableObject
    {
        [SerializeField]
        List<EnemyRecord> _enemyRecords = new List<EnemyRecord>();

        public EnemyParams GetParams(int grade, EnemyType enemyType)
        {
            var enemyRecord = _enemyRecords.FirstOrDefault(x => x.EnemyType == enemyType);
            if (enemyRecord == null)
            {
                Debug.LogError("Cant find enemyType " + enemyType);
                return null;
            }

            var enemyParams = enemyRecord.EnemyParams.FirstOrDefault(x => x.Grade == grade);
            if (enemyParams == null)
            {
                Debug.LogError("Cant find enemyParams for type " + enemyType + " grade " + grade);
                return null;
            }

            return enemyParams;
        }
    }
}

