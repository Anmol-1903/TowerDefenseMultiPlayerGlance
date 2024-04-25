using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Core.PathHandler;
using InputOwner = Core.GameEnums.OwnershipType;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Tower;
using Util;

/*
 * TODO: Change the Script name to better name as it will handle other player stuffs
 *      - Inputs
 *      - Tower Changing
 */

namespace Core.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputOwner owner;
        [SerializeField] private LayerMask exlcudedLayer;
        private TowerBase towerBase;
        private bool isValid;

        private TowerChangeHandler towerChangeHandler;

        private void Start()
        {
            GameManager.Instance.OnGameStart += (i) => EnableInputs();
            GameManager.Instance.OnGameEnd += (b) => DisableInputs();
            towerChangeHandler = FindFirstObjectByType<TowerChangeHandler>();
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
            if (RaycastFromFinger(finger, out RaycastHit hit))
            {
                // Check if the collider belongs to a GameObject with a TowerBase script

                if (hit.collider.TryGetComponent(out towerBase))
                {
                    if (towerBase.TowerOwner == owner)
                    {
                        if (towerBase.CanCreateConnections)
                        {
                            $"Finger touch {towerBase.TowerOwner} {towerBase.TowerType}".Log();
                            PathManager.Instance.GetHintLine(towerBase.transform.position);
                        }
                        if (towerBase.IsChangeable)
                        {
                            towerChangeHandler.OpenTowerInventory(towerBase, towerBase.transform.position);
                        }
                    }
                }
            }
        }

        private void Touch_onFingerMove(Finger finger)
        {
            if (towerBase != null && RaycastFromFinger(finger, out RaycastHit hit))
            {
                towerChangeHandler.CloseTowerInventory();
                RaycastHit[] hits = Physics.RaycastAll(towerBase.transform.position, hit.point - towerBase.transform.position, Mathf.Infinity, ~exlcudedLayer);
                isValid = hits.Length == 1
                    && hits[0].transform.GetComponent<TowerBase>() != null
                    && hit.transform.GetComponent<TowerBase>() != null
                    && hits[0].transform.GetComponent<TowerBase>().TowerID == hit.transform.GetComponent<TowerBase>().TowerID;
                PathManager.Instance.UpdateHintLine(hit.point, isValid);
            }
        }

        private void Touch_onFingerUp(Finger finger)
        {
            if (towerBase != null)
            {
                PathManager.Instance.RemoveHintLine();
                if (isValid)
                {
                    if (RaycastFromFinger(finger, out RaycastHit hit))
                    {
                        towerBase.ConnectTo(hit.transform.GetComponent<TowerBase>());
                    }
                }
                towerBase = null;
            }
        }

        public bool RaycastFromFinger(Finger finger, out RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(finger.screenPosition);
            return Physics.Raycast(ray, out hit, Mathf.Infinity);
        }
    }
}