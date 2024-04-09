using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "SceneContainer")]
    public class SceneContainerScriptable : ScriptableObject
    {
        [field: SerializeField, SceneName] public string MainMenuScene { get; private set; }
        [field: SerializeField, SceneName] public string[] TutorialLevelScenes { get; private set; }
        [field: SerializeField, SceneName] public string[] GameLevelScenes { get; private set; }
    }
}