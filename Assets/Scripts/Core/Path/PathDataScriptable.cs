using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.PathHandler
{
    [CreateAssetMenu(menuName = "Game/PathData")]
    public class PathDataScriptable : ScriptableObject
    {
        [field: SerializeField] public float OffsetY { get; private set; }
        [field: SerializeField, PrefabObjectOnly, NotNull] public Path PathRendererPrefab { get; private set; }
        [field: SerializeField, PrefabObjectOnly, NotNull] public LineRenderer HintLine { get; private set; }

        [field: SerializeField, InLineEditor] public Material ValidHintMaterial { get; private set; }
        [field: SerializeField, InLineEditor] public Material InValidMaterial { get; private set; }
        [field: SerializeField, LabelByChild("owner")] public PathVisual[] PathMaterial { get; private set; }
    }
}