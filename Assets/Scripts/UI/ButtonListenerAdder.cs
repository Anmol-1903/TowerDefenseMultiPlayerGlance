using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class ButtonListenerAdder : MonoBehaviour
    {
        [SerializeField] GameObject _mainMenu, _settings;
        //Handle UI display here generally
        public void Play()
        {
            MainMenuManager.Instance.PlayGame();
        }

        public void OpenSettings()
        {
            _mainMenu.SetActive(false);
            _settings.SetActive(true);
        }
        public void CloseSettings()
        {
            _mainMenu.SetActive(true);
            _settings.SetActive(false);
        }
    }
}