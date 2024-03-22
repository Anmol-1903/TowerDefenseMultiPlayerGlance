using System;
using UnityEngine;

namespace Tower
{
    [Serializable]
    public struct TowerConnection
    {
        public string Name;
        public LineRenderer Path;
        public TowerBase Tower;

        public TowerConnection(TowerBase tower, LineRenderer path)
        {
            Name = tower.name;
            Tower = tower;
            Path = path;
        }
    }
}