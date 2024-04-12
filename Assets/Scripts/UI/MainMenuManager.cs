using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
                ShowPopup("No Internet Connections");
            }
        }

        public void PlayGame()
        {
            GameManager.Instance.PlayGame();
        }

        /// <summary>
        /// Call this method to show popup in MainMenu
        /// </summary>
        /// <param name="msg">msg in PopUp</param>
        public void ShowPopup(string msg)
        {
        }
    }
}