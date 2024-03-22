using TowerType = Core.GameEnums.TowerType;

namespace Tower
{
    public class SoldierTower : TowerBase
    {
        protected override void Awake()
        {
            base.Awake();
            TowerType = TowerType.Soldier;
        }
    }
}