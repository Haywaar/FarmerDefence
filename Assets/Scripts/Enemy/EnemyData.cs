using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemyParams
{
    public int Grade;
    public float MovementSpeed;
    public int MaxHealth;
    public int DamageValue;
    public int RewardPrice;
    public GameObject Prefab;

    //TODO
    public Color MeshColor;
}

public enum EnemyType
{
    Elemental = 0,
    Bomb = 1,
    Troll = 2,
    Spider = 3,
    Mage = 4
}

[System.Serializable]
public class EnemyRecord
{
    public EnemyType EnemyType;
    public List<EnemyParams> EnemyParams;
}

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

