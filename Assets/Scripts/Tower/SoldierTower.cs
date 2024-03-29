using System;
using Troop;
using UnityEngine;
using TowerType = Core.GameEnums.TowerType;

namespace Tower
{
    public class SoldierTower : TowerBase
    {
        [SerializeField] private float soldierSpawnInterval;

        private float currentSpawnRate;

        protected override void Awake()
        {
            base.Awake();
            TowerType = TowerType.Soldier;
        }

        protected override void Spawn()
        {
        }

        protected override void Update()
        {
            base.Update();
            if (currentSpawnRate < 0)
            {
                Spawn();
                soldierSpawnInterval = currentSpawnRate;
            }
            else
            {
                currentSpawnRate -= Time.deltaTime;
            }
        }
    }
}