using System;
using UnityEngine;
using Owner = Core.GameEnums.OwnershipType;
using Tier = Core.GameEnums.Tier;

namespace Util
{
    [Serializable]
    public struct OwnerVisual
    {
        public Owner owner;
        public TierVisual[] tierVisuals;
    }

    [Serializable]
    public struct TierVisual
    {
        public Tier towerTier;
        [AssetPreview] public GameObject TowerLevelObject;
        [InLineEditor] public Material material;
    }

    [Serializable]
    public struct PathVisual
    {
        public Owner owner;
        public Material material;
    }
}