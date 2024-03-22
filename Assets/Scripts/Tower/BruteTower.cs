using TowerType = Core.GameEnums.TowerType;

namespace Tower
{
    public class BruteTower : TowerBase
    {
        protected override void Awake()
        {
            base.Awake();
            TowerType = TowerType.Brute;
        }
    }
}