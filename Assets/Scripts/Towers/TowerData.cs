using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
    Empty = 0,
    FastTower = 1,
    HeavyTower = 2,
    SplashTower = 3
}

//TODO change class to struct
[System.Serializable]
public class TowerParams
{
    public int Grade;
    public int DamageValue;
    public int UpgradePrice;
    public int SalvagePrice;
    public float AttackCooldown;
    public GameObject Prefab;
    public GameObject ShootPrefab;
}

[System.Serializable]
public class TowerRecord
{
    public TowerType towerType;
    public List<TowerParams> TowerParameters;
}

[CreateAssetMenu(fileName = "TowerData", menuName = "ScriptableObjects/TowerDataScriptableObject", order = 1)]
public class TowerData : ScriptableObject
{
    //Why i use 2 lists instead of one and serializing it?
    // Cause of unity overlapping editor text problem
    [SerializeField] private TowerRecord _fastTowerRecord = new TowerRecord(){towerType = TowerType.FastTower};
    [SerializeField] private TowerRecord _heavyTowerRecord = new TowerRecord(){towerType = TowerType.HeavyTower};
    [SerializeField] private TowerRecord _emptyTowerRecord = new TowerRecord(){towerType = TowerType.Empty};

    public TowerRecord EmptyTowerRecord => _emptyTowerRecord;

    public TowerRecord FastTowerRecord => _fastTowerRecord;
    public TowerRecord HeavyTowerRecord => _heavyTowerRecord;
}




