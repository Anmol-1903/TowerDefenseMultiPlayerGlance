using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Troop
{
    [CreateAssetMenu(menuName = "Game/TroopData")]
    public class TroopDataScriptable : ScriptableObject
    {
        [field: SerializeField] public float OffsetY { get; private set; }

        [field: SerializeField, BeginGroup("Soldier")] public int SoldierPoolSize { get; private set; }
        [field: SerializeField] public int SoldierMaxPoolSize { get; private set; }
        [field: SerializeField] public int SoldierHealth { get; private set; }
        [field: SerializeField] public int SoldierLevel { get; private set; }
        [field: SerializeField, AssetPreview, NotNull] public SoldierTroop SoldierPrefab { get; private set; }
        [field: SerializeField, EndGroup] public OwnerVisual[] SoldierVisual { get; private set; }
        [field: SerializeField, BeginGroup("Brute")] public int BrutePoolSize { get; private set; }
        [field: SerializeField] public int BruteMaxPoolSize { get; private set; }
        [field: SerializeField] public int BruteHealth { get; private set; }
        [field: SerializeField] public int BruteLevel { get; private set; }
        [field: SerializeField, AssetPreview, NotNull] public BruteTroop BrutePrefab { get; private set; }
        [field: SerializeField, EndGroup] public OwnerVisual[] BruteVisual { get; private set; }
    }
}