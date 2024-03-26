using System;
using System.Collections.Generic;
using UnitySingleton;
using UnityEngine;
using System.Linq;

using OwnershipType = Core.GameEnums.OwnershipType;
using TowerType = Core.GameEnums.TowerType;

namespace Tower
{
    public class TowerTracker : MonoSingleton<TowerTracker>
    {
        [SerializeField] private List<TowerBase> towerList;
        [SerializeField, LabelByChild("OwnershipType")] private List<TowerByOwner> towersByOwner;
        [SerializeField, LabelByChild("Type")] private List<TowerByType> towersByType;

        /// <summary>
        /// Index of <br></br>
        /// <br></br>
        /// <see cref="Core.GameEnums.OwnershipType.UnConquered"/> : <b>0</b><br></br>
        /// <see cref="Core.GameEnums.OwnershipType.Blue"/> : <b>1</b><br></br>
        /// <see cref="Core.GameEnums.OwnershipType.Red"/> : <b>2</b><br></br>
        /// <see cref="Core.GameEnums.OwnershipType.Green"/> : <b>3</b><br></br>
        /// <see cref="Core.GameEnums.OwnershipType.Yellow"/> : <b>4</b>
        /// </summary>
        public List<TowerByOwner> TowersByOwner => towersByOwner;

        /// <summary>
        /// Index of <br></br>
        /// <br></br>
        /// <see cref="Core.GameEnums.TowerType.Soldier"/> : <b>0</b><br></br>
        /// <see cref="Core.GameEnums.TowerType.Brute"/> : <b>1</b><br></br>
        /// <see cref="Core.GameEnums.TowerType.Archery"/> : <b>2</b><br></br>
        /// <see cref="Core.GameEnums.TowerType.Fort"/> : <b>3</b><br></br>
        ///  /// <see cref="Core.GameEnums.TowerType.Base"/> : <b>4</b><br></br>
        /// </summary>
        public List<TowerByType> TowersByType { get => towersByType; }

        private void Start()
        {
            GetAllTower();
            FilterTowerByOwner();
            FilterTowerByType();
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

        public void FilterTowerByType()
        {
            towersByType = new(4)
            {
                new TowerByType(TowerType.Soldier,new (GetTowerByType(TowerType.Soldier))),
                new TowerByType(TowerType.Brute,new (GetTowerByType(TowerType.Brute))),
                new TowerByType(TowerType.Archery,new (GetTowerByType(TowerType.Archery))),
                new TowerByType (TowerType.Fort,new (GetTowerByType(TowerType.Fort))),
                new TowerByType (TowerType.Base,new (GetTowerByType(TowerType.Base))),
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

        public List<TowerBase> GetTowerByType(TowerType type)
        {
            List<TowerBase> lst = new();
            if (towerList == null || towerList.Count == 0)
            {
                GetAllTower();
            }
            foreach (var tower in towerList)
            {
                if (tower.TowerType == type)
                {
                    lst.Add(tower);
                }
            }
            return lst;
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

        public TowerByType(TowerType type, List<TowerBase> towers)
        {
            Type = type;
            Towers = towers;
        }
    }
}