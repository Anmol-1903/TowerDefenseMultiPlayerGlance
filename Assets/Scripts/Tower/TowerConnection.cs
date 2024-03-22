using Core.PathHandler;
using System;
using UnityEngine;

namespace Tower
{
    [Serializable]
    public struct TowerConnection
    {
        public string Name;
        public Path TowerPath;
        public TowerBase Tower;

        public TowerConnection(TowerBase tower, Path path)
        {
            Name = tower.name;
            Tower = tower;
            TowerPath = path;
        }
    }
}