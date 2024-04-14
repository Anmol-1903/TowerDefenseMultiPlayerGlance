using Troop;
using UnityEngine;
using Util;
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

        protected override void Start()
        {
            base.Start();
            currentSpawnRate = soldierSpawnInterval;
        }

        protected override void Spawn()
        {
            if (Connections.Count > 0)
            {
                foreach (var connection in Connections)
                {
                    TroopPooler.Instance.SpawnSoldierTroop(TowerID, connection.Tower.TowerID, TowerOwner, transform.position, connection.Tower.transform.position);
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            if (currentSpawnRate < 0)
            {
                Spawn();
                currentSpawnRate = soldierSpawnInterval;
            }
            else
            {
                currentSpawnRate -= Time.deltaTime;
            }
        }
    }
}