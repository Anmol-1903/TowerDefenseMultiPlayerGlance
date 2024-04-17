using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioMixer soundMixer, musicMixer;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 AudioManager in the Scene");
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);

        soundMixer.SetFloat("Sound", PlayerPrefs.GetInt("Sound", 0));
        musicMixer.SetFloat("Music", PlayerPrefs.GetInt("Music", 0));
    }

    public void SetSounds(int _vol)
    {
        soundMixer.SetFloat("Sound", _vol);
    }
    public void SetMusic(int _vol)
    {
        musicMixer.SetFloat("Music", _vol);
    }
}