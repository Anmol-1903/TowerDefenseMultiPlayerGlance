using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Game/AudioContainer")]
    public class AudioContainer : ScriptableObject
    {
        [field: SerializeField, LabelByChild("name")] public BackGroundMusic[] BackGroundMusics { get; private set; }
        [field: SerializeField, LabelByChild("name")] public Sfx[] SfxAudios { get; private set; }
    }

    [System.Serializable]
    public struct BackGroundMusic
    {
        public string name;
        [InLineEditor] public AudioClip audioClip;
    }

    [System.Serializable]
    public struct Sfx
    {
        public string name;
        [InLineEditor] public AudioClip audioClip;
    }
}