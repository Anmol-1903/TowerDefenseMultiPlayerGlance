using Troop;
using UnityEngine;
using TowerType = Core.GameEnums.TowerType;

namespace Tower
{
    public class BruteTower : TowerBase
    {
        [SerializeField] private float bruteSpawnInterval;
        private float currentBruteSpawnInterval;

        protected override void Awake()
        {
            base.Awake();
            TowerType = TowerType.Brute;
        }

        protected override void Update()
        {
            base.Update();
            if (currentBruteSpawnInterval < 0)
            {
                Spawn();
                currentBruteSpawnInterval = bruteSpawnInterval;
            }
            else
            {
                currentBruteSpawnInterval -= Time.deltaTime;
            }
        }

        protected override void Spawn()
        {
            if (Connections.Count > 0)
            {
                foreach (var connection in Connections)
                {
                    if (connection.Tower == null)
                        return;
                    TroopPooler.Instance.SpawnBruteTroop(TowerID, connection.Tower.TowerID, TowerOwner, transform.position, connection.Tower.transform.position);
                }
            }
        }
    }
}