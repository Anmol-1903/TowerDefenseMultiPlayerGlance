using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySingleton;

namespace Core
{
    public class AudioManager : PersistentMonoSingleton<AudioManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }
    }
}