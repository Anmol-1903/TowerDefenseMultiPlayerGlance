using TowerType = Core.GameEnums.TowerType;

namespace Tower
{
    public class ArcheryTower : TowerBase
    {
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
        }

        protected override void ConnectionCheckUpdate()
        {
            return;
        }
    }
}