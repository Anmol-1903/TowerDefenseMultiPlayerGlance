using UnityEngine;
using TowerType = Core.GameEnums.TowerType;

namespace Tower
{
    public class ArcheryTower : TowerBase
    {
        [SerializeField] private float attackInterval;
        private float currentAttackInterval;

        protected override void Awake()
        {
            base.Awake();
            TowerType = TowerType.Archery;
            CanCreateConnections = false;
            usedPaths = maxPaths = 0;
        }

        protected override void Update()
        {
            base.Update();
            if (currentAttackInterval < 0)
            {
                Spawn();
                currentAttackInterval = attackInterval;
            }
            else
            {
                currentAttackInterval -= Time.deltaTime;
            }
        }

        protected override void ConnectionCheckUpdate()
        {
            return;
        }

        protected override void Spawn()
        {
            //spawn attack/projectile
        }
    }
}