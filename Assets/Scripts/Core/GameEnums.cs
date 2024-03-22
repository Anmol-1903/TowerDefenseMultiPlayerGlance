using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySingleton;

namespace Core
{
    public class GameEnums : Singleton<GameEnums>
    {
        public enum PlayerType
        {
            Blue,
            Red,
            Green,
            Yellow
        }

        public enum TowerType
        {
            Soldier,
            Brute,
            Archery,
            Fort
        }
    }
}