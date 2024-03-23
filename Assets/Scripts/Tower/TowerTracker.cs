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
        [field: SerializeField] public List<TowerBase> TowerList { get; private set; }

        private void Start()
        {
            GetAllTower();
        }

        private void GetAllTower()
        {
            TowerList = new();
            TowerList.AddRange(FindObjectsByType<TowerBase>(FindObjectsSortMode.None).ToList());
        }
    }

    [Serializable]
    public struct TowerByOwner
    {
        public OwnershipType OwnershipType;
        public List<TowerBase> Towers;
    }

    [Serializable]
    public struct TowerByType
    {
        public TowerType Type;
        public List<TowerBase> Towers;
    }
}