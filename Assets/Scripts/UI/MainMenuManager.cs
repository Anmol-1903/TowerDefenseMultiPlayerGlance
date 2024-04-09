using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySingleton;

namespace UI
{
    public class MainMenuManager : MonoSingleton<MainMenuManager>
    {
        //Handle Main Menu Logic
        protected override void Awake()
        {
            base.Awake();
        }

        public void PlayGame()
        {
        }
    }
}