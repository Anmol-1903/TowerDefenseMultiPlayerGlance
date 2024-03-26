using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using InputOwner = Core.GameEnums.OwnershipType;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Core.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputOwner owner;

        private void OnEnable()
        {
            GameManager.Instance.OnGameStart += EnableInputs;
            GameManager.Instance.OnGameStop += (bool b) => DisableInputs();
        }

        private void OnDisable()
        {
            GameManager.Instance.OnGameStart -= EnableInputs;
            GameManager.Instance.OnGameStop -= (bool b) => DisableInputs();
        }

        private void EnableInputs()
        {
            EnhancedTouchSupport.Enable();
            Touch.onFingerDown += Touch_onFingerDown;
            Touch.onFingerMove += Touch_onFingerMove;
            Touch.onFingerUp += Touch_onFingerUp;
        }

        private void DisableInputs()
        {
            Touch.onFingerDown -= Touch_onFingerDown;
            Touch.onFingerMove -= Touch_onFingerMove;
            Touch.onFingerUp -= Touch_onFingerUp;
            EnhancedTouchSupport.Disable();
        }

        private void Touch_onFingerDown(Finger finger)
        {
        }

        private void Touch_onFingerMove(Finger finger)
        {
            throw new NotImplementedException();
        }

        private void Touch_onFingerUp(Finger finger)
        {
            throw new NotImplementedException();
        }
    }
}