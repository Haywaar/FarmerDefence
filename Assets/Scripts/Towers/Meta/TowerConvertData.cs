using System.Collections.Generic;
using UnityEngine;

namespace Towers.Meta
{
    [CreateAssetMenu(fileName = "TowerConvertData", menuName = "ScriptableObjects/TowerConvertDataScriptableObject", order = 1)]
    public class TowerConvertData : ScriptableObject
    {
        [SerializeField] private List<ConvertRecord> _convertRecords;

        public List<ConvertRecord> ConvertRecords => _convertRecords;
    }
}
