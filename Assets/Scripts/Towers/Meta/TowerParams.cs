using UnityEngine;

namespace Towers.Meta
{
    [System.Serializable]
    public class TowerParams
    {
        public int Grade;
        public int DamageValue;
        public int UpgradePrice;
        public int SalvagePrice;
        public float AttackCooldown;
        public float Radius;
        public GameObject Prefab;
        public Sprite TowerIcon;
        public GameObject ShootPrefab;
    }
}