using System.Collections.Generic;

namespace Towers.Meta
{
    [System.Serializable]
    public class TowerRecord
    {
        public TowerType towerType;
        public List<TowerParams> TowerParameters;
    }
}