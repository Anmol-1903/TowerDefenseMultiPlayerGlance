using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySingleton;

using ConnectionStatus = Core.GameEnums.ConnectionStatus;

namespace UI
{
    public class MainMenuManager : MonoSingleton<MainMenuManager>
    {
        //Handle Main Menu Logic
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            if (GameManager.Instance.ConStatus == ConnectionStatus.Disconnected)
            {
                ShowOfflinePopup();
            }
        }

        public void PlayGame()
        {
        }

        private void ShowOfflinePopup()
        {
        }
    }
}