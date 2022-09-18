using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Towers;
using UnityEngine;
using Zenject;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private TowerData _towerData;
    [SerializeField] private TowerConvertData _towerConvertData;

    [Header("Set automatically, dont inject!")] [SerializeField]
    private List<Tower> _towers = new List<Tower>();

    public event Action<int> OnTowerClicked;
    private List<TowerRecord> _towerRecords;
    private Player _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }


    private void Awake()
    {
        _towerRecords = new List<TowerRecord>();
        _towerRecords.Add(_towerData.HeavyTowerRecord);
        _towerRecords.Add(_towerData.FastTowerRecord);
        _towerRecords.Add(_towerData.EmptyTowerRecord);
    }
    
    public int Register(Tower tower)
    {
        _towers.Add(tower);
        return _towers.IndexOf(tower);
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

    #region Upgrade

    public bool IsUpgradable(int towerId)
    {
        var tower = GetTowerById(towerId);
        return tower.TowerType != TowerType.Empty;
    }

    private bool CanUpgradeTower(Tower tower)
    {
        return _player.Money >= tower.Params.UpgradePrice;
    }

    public void TryUpgradeTower(int towerId)
    {
        var tower = GetTowerById(towerId);
        var towerType = tower.TowerType;
        var grade = tower.Grade + 1;

        if (CanUpgradeTower(tower))
        {
            var towerParams = GetParams(towerType, grade);
            _player.ReduceMoney(towerParams.UpgradePrice);
            tower.Upgrade(towerParams);
        }
    }

    #endregion

    #region Convert

    private bool CanConvertTower(int towerId, TowerType newTowerType)
    {
        return _player.Money >= GetConvertPrice(towerId, newTowerType);
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

    public int GetConvertPrice(int towerId, TowerType newTowerType)
    {
        var convertRecord = GetConvertPrices(towerId).FirstOrDefault(x => x.toTowerType == newTowerType);
        if (convertRecord == null)
        {
            Debug.LogError("Can't find price for convert tower with id  " + towerId + " to type " + newTowerType);
            return 0;
        }

        return convertRecord.convertPrice;
    }

    public bool TryConvert(int towerId, TowerType towerType, int grade)
    {
        bool convertSuccess = false;
        var tower = GetTowerById(towerId);

        if (CanConvertTower(towerId, towerType))
        {
            _player.ReduceMoney(GetConvertPrice(towerId, towerType));

            var towerParams = GetParams(towerType, grade);
            tower.ChangeType(towerType);
            tower.Upgrade(towerParams);
            convertSuccess = true;
        }

        return convertSuccess;
    }

    #endregion

    public void SalvageTower(int towerId)
    {
        var tower = GetTowerById(towerId);
        var salvagePrice = tower.Params.SalvagePrice;
        tower.Salvage();
        _player.AddMoney(salvagePrice);
    }

    public void TowerClicked(int towerId)
    {
        //TODO
        OnTowerClicked?.Invoke(towerId);
        if (towerId >= 0)
        {
            SelectTower(towerId);
        }
        else
        {
            DeselectAllTowers();
        }
    }

    private void SelectTower(int towerId)
    {
        foreach (var tower in _towers)
        {
            bool isSelected = tower.TowerId == towerId;
            tower.SetSelected(isSelected);
        }
    }

    private void DeselectAllTowers()
    {
        foreach (var tower in _towers)
        {
            tower.SetSelected(false);
        }
    }
}