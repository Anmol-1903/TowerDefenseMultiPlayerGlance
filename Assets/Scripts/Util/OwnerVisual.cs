using System;
using UnityEngine;
using Owner = Core.GameEnums.OwnershipType;

namespace Util
{
    [Serializable]
    public struct OwnerVisual
    {
        public Owner owner;
        [InLineEditor] public Material material;
    }
}