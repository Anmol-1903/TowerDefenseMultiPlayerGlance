using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Core.PathHandler;
using InputOwner = Core.GameEnums.OwnershipType;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Tower;
using Util;

namespace Core.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputOwner owner;
        [SerializeField] private LayerMask exlcudedLayer;
        private TowerBase towerBase;
        private bool isValid;

        private void Start()
        {
            GameManager.Instance.OnGameStart += EnableInputs;
            GameManager.Instance.OnGameEnd += (b) => DisableInputs();
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
                    if (towerBase.TowerOwner == owner && towerBase.CanCreateConnections)
                    {
                        $"Finger touch {towerBase.TowerOwner} {towerBase.TowerType}".Log();
                        PathManager.Instance.GetHintLine(towerBase.transform.position);
                    }
                }
            }
        }

        private void Touch_onFingerMove(Finger finger)
        {
            if (towerBase != null && RaycastFromFinger(finger, out RaycastHit hit))
            {
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

        private Vector3 IsoVectorConvertor(Vector3 vector)
        {
            Quaternion rotation = Quaternion.Euler(0, 45, 0);
            Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
            return isoMatrix.MultiplyPoint3x4(vector);
        }
    }
}