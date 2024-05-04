using Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class UIListenerAdder : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu, _settings;

        [SerializeField] private TMP_InputField nicknameField;

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

        public void UpdateNickname(string input)
        {
            GameManager.Instance.GameSetting.DisplayNickName = input;
            GameManager.Instance.GameSetting.SaveData();
        }

        /*
                public void EnableSFX(bool isEnable)
                {
                    AudioManager.Instance.
                }*/
    }
}