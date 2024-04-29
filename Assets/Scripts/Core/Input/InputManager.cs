using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Core.PathHandler;
using InputOwner = Core.GameEnums.OwnershipType;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Tower;
using Util;
using Photon.Pun;

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

        private PhotonView photonView;

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            GameManager.Instance.OnGameStart += EnableInputs;
            GameManager.Instance.OnGameEnd += (b) => DisableInputs();
            towerChangeHandler = FindFirstObjectByType<TowerChangeHandler>();
        }

        private void OnGameStart()
        {
            // Check if this instance is owned by the local player
            if (photonView.IsMine)
            {
                // Get the owner type for this client based on its index
                int clientIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // Subtract 1 to make index zero-based
                InputOwner clientOwner = GetClientOwnerFromIndex(clientIndex);

                // Spawn the input manager with the appropriate owner for this client
                photonView.RPC("SpawnInputManager", RpcTarget.AllBuffered, clientOwner);
            }
        }

        private void OnGameEnd(bool success)
        {
            // Clean up when the game ends
            if (photonView.IsMine)
            {
                // Perform cleanup logic
            }
        }

        [PunRPC]
        private void SpawnInputManager(InputOwner clientOwner)
        {
            // Set the owner for this input manager
            owner = clientOwner;

            // Enable inputs only if this instance is owned by the local player
            if (photonView.IsMine)
            {
                EnableInputs();
            }
            else
            {
                DisableInputs();
            }
        }

        private InputOwner GetClientOwnerFromIndex(int clientIndex)
        {
            // Logic to determine owner type based on client index
            // Example: Assign blue owner to the first client, red to the second, and so on...
            switch (clientIndex)
            {
                case 0: return InputOwner.Blue;
                case 1: return InputOwner.Red;
                case 2: return InputOwner.Yellow;
                case 3: return InputOwner.Green;
                default: return InputOwner.Blue; // Default to Blue if index is out of range
            }
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
            "Inputs".Log();
            if (RaycastFromFinger(finger, out RaycastHit hit))
            {
                // Check if the collider belongs to a GameObject with a TowerBase script

                if (hit.collider.transform.root.TryGetComponent(out towerBase))
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
                    && hits[0].transform.root.GetComponent<TowerBase>() != null
                    && hit.transform.root.GetComponent<TowerBase>() != null
                    && hits[0].transform.root.GetComponent<TowerBase>().TowerID == hit.transform.root.GetComponent<TowerBase>().TowerID;
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
                    if (RaycastFromFinger(finger, out RaycastHit hit)) //Use tryget
                    {
                        towerBase.ConnectTo(hit.transform.root.GetComponent<TowerBase>());
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