using UnityEngine;

namespace Towers.Meta
{
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
}




