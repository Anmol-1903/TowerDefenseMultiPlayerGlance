using System;
using UnityEngine;
using PathOwner = Core.GameEnums.PlayerType;

namespace Core.PathHandler
{
    [Serializable]
    public struct PathVisual
    {
        public PathOwner owner;
        [InLineEditor] public Material material;
    }
}