using UnityEngine;

namespace Enemy.Meta
{
    [System.Serializable]
    public class EnemyParams
    {
        public int Grade;
        public float MovementSpeed;
        public int MaxHealth;
        public int DamageValue;
        public int RewardPrice;
        public GameObject Prefab;

        public Material Material;
    }
}