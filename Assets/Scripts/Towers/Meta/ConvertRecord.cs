using Towers.Meta;

[System.Serializable]
public class ConvertRecord
{
    public TowerType fromTowerType;
    public int fromGrade;
    
    public TowerType toTowerType;
    public int toGrade;

    public int convertPrice;
}