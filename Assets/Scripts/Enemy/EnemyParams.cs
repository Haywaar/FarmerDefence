using System.Collections;
using System.Collections.Generic;
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
}

