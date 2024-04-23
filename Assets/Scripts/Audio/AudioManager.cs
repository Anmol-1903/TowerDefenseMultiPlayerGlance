using Core.SaveLoadSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySingleton;

namespace Audio
{
    public class AudioManager : PersistentMonoSingleton<AudioManager>
    {
        [field: SerializeField] public AudioContainer container;
        public bool CanPlayBackgroundMusic { get; private set; }
        public bool CanPlaySfx { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            //todo: Load Audio Settings for SaveFiles or GameManager.Instance.GameSettings
        }

        public void LoadContainer(AudioContainer data)
        {
            container = data;
        }

        public void PlayBGM(string name, AudioSource source)
        {
            source.clip = Array.Find(container.BackGroundMusics, musics => musics.name == name).audioClip;
            source.Play();
        }

        public void PlaySfx(string name, AudioSource source)
        {
            source.clip = Array.Find(container.SfxAudios, musics => musics.name == name).audioClip;
            source.Play();
        }
    }
}