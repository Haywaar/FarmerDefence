using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ConvertRecord
{
    public TowerType fromTowerType;
    public int fromGrade;
    
    public TowerType toTowerType;
    public int toGrade;

    public int convertPrice;
}

[CreateAssetMenu(fileName = "TowerConvertData", menuName = "ScriptableObjects/TowerConvertDataScriptableObject", order = 1)]
public class TowerConvertData : ScriptableObject
{
    [SerializeField] private List<ConvertRecord> _convertRecords;

    public List<ConvertRecord> ConvertRecords => _convertRecords;
}
