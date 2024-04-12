using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class ButtonListenerAdder : MonoBehaviour
    {
        //Handle UI display here generally
        public void Play()
        {
            MainMenuManager.Instance.PlayGame();
        }

        public void OpenSettings()
        {
        }
    }
}