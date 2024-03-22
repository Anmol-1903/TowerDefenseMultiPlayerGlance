using UnitySingleton;

namespace Core
{
    public class GameEnums : Singleton<GameEnums>
    {
        public enum OwnershipType
        {
            UnConquered,
            Blue,
            Red,
            Green,
            Yellow,
        }

        public enum TowerType
        {
            Soldier,
            Brute,
            Archery,
            Fort
        }

        public enum Tier
        {
            Tier1 = 1,
            Tier2 = 2,
            Tier3 = 3,
        }
    }
}