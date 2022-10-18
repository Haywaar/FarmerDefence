using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace Towers
{
    public class TowerManager : MonoBehaviour
    {
        [SerializeField] private TowerData _towerData;
        [SerializeField] private TowerConvertData _towerConvertData;

        public List<Tower> Towers { get; } = new List<Tower>();
        public event Action<int> OnTowerClicked;
        private List<TowerRecord> _towerRecords;
        
        private Player _player;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(Player player, SignalBus signalBus)
        {
            _player = player;
            _signalBus = signalBus;
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
            Towers.Add(tower);
            return Towers.IndexOf(tower);
        }
    
        private Tower GetTowerById(int towerId)
        {
            var tower = Towers.FirstOrDefault(x => x.TowerId == towerId);
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

        //TODO - refactor to upgrade logic
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
                _signalBus.Fire(new ChangeMoneySignal(-1 * towerParams.UpgradePrice));
                tower.Upgrade(towerParams);
            }
        }

        #endregion

        //TODO - refactor to convert logic module
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

        public Sprite GetTypeSprite(TowerType towerType, int grade = 1)
        {
            var towerParams = GetParams(towerType, grade);
            return towerParams.TowerIcon;
        }
    
        public Sprite GetNextGradeSprite(int towerId)
        {
            var tower = GetTowerById(towerId);
            return  GetParams(tower.TowerType, tower.Grade + 1).TowerIcon;
        }

        public bool TryConvert(int towerId, TowerType towerType, int grade)
        {
            bool convertSuccess = false;
            var tower = GetTowerById(towerId);

            if (CanConvertTower(towerId, towerType))
            {
                _signalBus.Fire(new ChangeMoneySignal(-1 * GetConvertPrice(towerId, towerType)));

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
            _signalBus.Fire(new ChangeMoneySignal(salvagePrice));
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
            foreach (var tower in Towers)
            {
                bool isSelected = tower.TowerId == towerId;
                tower.SetSelected(isSelected);
            }
        }

        private void DeselectAllTowers()
        {
            foreach (var tower in Towers)
            {
                tower.SetSelected(false);
            }
        }
    }
}