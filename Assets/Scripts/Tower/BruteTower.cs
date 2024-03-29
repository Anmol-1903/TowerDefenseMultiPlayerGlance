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
        }
    }
}