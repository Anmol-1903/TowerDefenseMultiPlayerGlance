using System;
using System.Collections.Generic;
using UnitySingleton;
using UnityEngine;
using OwnershipType = Core.GameEnums.OwnershipType;
using TowerType = Core.GameEnums.TowerType;
using System.Linq;

namespace Tower
{
    public class TowerTracker : MonoSingleton<TowerTracker>
    {
        [SerializeField] private List<TowerBase> towerList;
        [SerializeField, LabelByChild("OwnershipType")] private List<TowerByOwner> towersByOwner;

        public List<TowerByOwner> TowersByOwner => towersByOwner;

        private void Start()
        {
            GetAllTower();
            FilterTowerByOwner();
        }

        public List<TowerBase> GetAllTower()
        {
            towerList = new();
            towerList.AddRange(FindObjectsByType<TowerBase>(FindObjectsSortMode.None).ToList());
            return towerList;
        }

        public void FilterTowerByOwner()
        {
            towersByOwner = new(5)
            {
                new TowerByOwner(OwnershipType.UnConquered,new (GetTowerByOwner(OwnershipType.UnConquered))),
                new TowerByOwner(OwnershipType.Blue,new (GetTowerByOwner(OwnershipType.Blue))),
                new TowerByOwner(OwnershipType.Red,new (GetTowerByOwner(OwnershipType.Red))),
                new TowerByOwner(OwnershipType.Green,new (GetTowerByOwner(OwnershipType.Green))),
                new TowerByOwner(OwnershipType.Yellow,new (GetTowerByOwner(OwnershipType.Yellow)))
            };
        }

        public List<TowerBase> GetTowerByOwner(OwnershipType ownerType)
        {
            List<TowerBase> list = new();
            if (towerList == null || towerList.Count == 0)
            {
                GetAllTower();
            }
            foreach (var tower in towerList)
            {
                if (tower.TowerOwner == ownerType)
                {
                    list.Add(tower);
                }
            }
            return list;
        }
    }

    [Serializable]
    public struct TowerByOwner
    {
        public OwnershipType OwnershipType;
        public List<TowerBase> Towers;

        public TowerByOwner(OwnershipType ownershipType, List<TowerBase> towers)
        {
            OwnershipType = ownershipType;
            Towers = towers;
        }
    }

    [Serializable]
    public struct TowerByType
    {
        public TowerType Type;
        public List<TowerBase> Towers;
    }
}