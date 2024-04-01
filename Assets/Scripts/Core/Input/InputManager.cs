using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Core.PathHandler;
using InputOwner = Core.GameEnums.OwnershipType;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Util;
using Tower;

namespace Core.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputOwner owner;
        private TowerBase towerBase;
        private void OnEnable()
        {
            /*            GameManager.Instance.OnGameStart += EnableInputs;
                        GameManager.Instance.OnGameStop += (bool b) => DisableInputs();*/

            EnableInputs();
        }

        private void OnDisable()
        {
            /*            GameManager.Instance.OnGameStart -= EnableInputs;
                        GameManager.Instance.OnGameStop -= (bool b) => DisableInputs();*/
            DisableInputs();
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
            RaycastHit hit;
            if (RaycastFromFinger(finger, out hit))
            {
                // Check if the collider belongs to a GameObject with a TowerBase script
                towerBase = hit.collider.GetComponent<TowerBase>();
                if (towerBase != null && towerBase.CanCreateConnections)
                {
                    Debug.Log("Finger touched a tower!");
                    PathManager.Instance.GetHintLine(towerBase.transform.position);

                }
            }
        }

        private void Touch_onFingerMove(Finger finger)
        {
            if (towerBase != null)
            {
                PathManager.Instance.UpdateHintLine(Camera.main.ScreenToWorldPoint(finger.screenPosition), true);
            }
        }

        private void Touch_onFingerUp(Finger finger)
        {
            if (towerBase != null)
            {
                PathManager.Instance.RemoveHintLine();
            }
        }

        public static bool RaycastFromFinger(Finger finger, out RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(finger.screenPosition);
            return Physics.Raycast(ray, out hit);
        }
    }
}