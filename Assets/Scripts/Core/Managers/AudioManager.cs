using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySingleton;

namespace Core
{
    public class AudioManager : PersistentMonoSingleton<AudioManager>
    {
        public bool CanPlayBackgroundMusic { get; private set; }
        public bool CanPlaySfx { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            //todo: Load Audio Settings for SaveFiles or GameManager.Instance.GameSettings
        }
    }
}