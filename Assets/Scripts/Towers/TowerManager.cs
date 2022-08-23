using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Towers;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private TowerData _towerData;
    [SerializeField] private TowerConvertData _towerConvertData;

    [Header("Set automatically, dont inject!")]
    [SerializeField] private List<Tower> _towers = new List<Tower>();
    
    public event Action<int> OnTowerClicked;
    private List<TowerRecord> _towerRecords;
   
    public bool IsUpgradable(int towerId)
    {
        var tower = GetTowerById(towerId);
        return tower.TowerType != TowerType.Empty;
    }
    private bool CanUpgradeTower(Tower tower)
    {
        return Player.Instance.Money >= tower.Params.UpgradePrice;
    }

    public void TryUpgradeTower(int towerId)
    {
        var tower = GetTowerById(towerId);
        var towerType = tower.TowerType;
        var grade = tower.Grade + 1;

        if (CanUpgradeTower(tower))
        {
            var towerParams = GetParams(towerType, grade);
            Player.Instance.ReduceMoney(towerParams.UpgradePrice);
            tower.Upgrade(towerParams);
        }
    }

    public bool IsConvertable(int towerId)
    {
        var tower = GetTowerById(towerId);
        return tower.TowerType == TowerType.Empty;
    }

    public List<ConvertRecord> GetConvertPrices(int towerId)
    {
        var tower = GetTowerById(towerId);
        return _towerConvertData.ConvertRecords
            .Where(x => x.fromTowerType == tower.TowerType && x.fromGrade == tower.Grade).ToList();
    }

    public void TryChangeTowerType(int towerId, TowerType towerType, int grade)
    {
        var tower = GetTowerById(towerId);
        
        var towerParams = GetParams(towerType, grade);
        tower.ChangeType(towerType);
        tower.Upgrade(towerParams);
        //TODO
    }

    private Tower GetTowerById(int towerId)
    {
        var tower = _towers.FirstOrDefault(x => x.TowerId == towerId);
        if (tower == null)
        {
            Debug.LogError("Cant find tower by id");
        }

        return tower;
    }

    public TowerParams GetParams(TowerType type, int grade)
    {
        var towerRecord = _towerRecords.FirstOrDefault(x => x.towerType == type);
        if (towerRecord == null)
        {
            Debug.LogError("Something wrong! couldnt find tower record for " + type + " grade " + grade);
            return null;
        }

        var towerParam = towerRecord.TowerParameters.FirstOrDefault(x => x.Grade == grade);
        if (towerParam == null)
        {
            Debug.LogError("Something wrong! couldnt find towerParam for " + type + " grade " + grade);
        }
        return towerParam;
    }

    public TowerParams GetParams(int towerId)
    {
        var tower = GetTowerById(towerId);
        return tower.Params;
    }

    public void TowerClicked(int towerId)
    {
        //TODO
        SelectTower(towerId);
        OnTowerClicked?.Invoke(towerId);
    }

    public void SalvageTower(int towerId)
    {
        var tower = GetTowerById(towerId);
        var salvagePrice = tower.Params.SalvagePrice;
        tower.Salvage();
        Player.Instance.AddMoney(salvagePrice);
    }

    public int Register(Tower tower)
    {
        _towers.Add(tower);
        return _towers.IndexOf(tower);
    }

    private TowerManager()
    {
    }

    public static TowerManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _towerRecords = new List<TowerRecord>();
            _towerRecords.Add(_towerData.HeavyTowerRecord);
            _towerRecords.Add(_towerData.FastTowerRecord);
            _towerRecords.Add(_towerData.EmptyTowerRecord);
        }
        else
        {
            Destroy(this);
        }
    }

    public void SelectTower(int towerId)
    {
        foreach (var tower in _towers)
        {
            bool isSelected = tower.TowerId == towerId;
            tower.SetSelected(isSelected);
        }
    }
}
