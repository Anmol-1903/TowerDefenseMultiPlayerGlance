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
            Fort,
            Base
        }

        public enum Tier
        {
            Tier1 = 1,
            Tier2 = 2,
            Tier3 = 3,
        }

        public enum ConnectionStatus
        {
            Connected,
            Disconnected,
            IDK
        }

        public enum TowerChangeability
        {
            Changeable,
            NonChangeable
        }
    }
}