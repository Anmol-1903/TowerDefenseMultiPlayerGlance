using UnityEngine;
using System.Collections;
using TowerType = Core.GameEnums.TowerType;
using Troop;

namespace Tower
{
    public class RocketLauncherTower : TowerBase
    {
        public bool IsActivelyShooting { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            TowerType = TowerType.Rocket;
            CanCreateConnections = false;
            UsedPaths = MaxPaths = 0;
        }

        public void Retarget(Vector3 pos)
        {
        }

        protected override void Spawn()
        {
            throw new System.NotImplementedException();
        }
    }
}