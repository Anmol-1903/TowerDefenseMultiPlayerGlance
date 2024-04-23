using UnityEngine;
using UnityEngine.UI;
using Audio;

namespace UI
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private Toggle sound, music, vibration;

        public void SetSoundVolume(bool _bool)
        {
            //AudioManager.Instance.SetSounds(_bool ? 0 : -80);
        }

        public void SetMusicVolume(bool _bool)
        {
            //AudioManager.Instance.SetMusic(_bool ? 0 : -80);
        }

        public void SetVibration(bool _bool)
        {
            PlayerPrefs.SetInt("Vibration", _bool ? 1 : 0);
        }
    }
}