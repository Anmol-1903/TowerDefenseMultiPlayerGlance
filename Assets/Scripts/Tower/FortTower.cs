using TowerType = Core.GameEnums.TowerType;

namespace Tower
{
    public class FortTower : TowerBase
    {
        protected override void Awake()
        {
            base.Awake();
            TowerType = TowerType.Fort;
        }

        protected override void Spawn()
        {
            return;
        }
    }
}