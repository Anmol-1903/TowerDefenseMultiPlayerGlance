using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Core.PathHandler;
using InputOwner = Core.GameEnums.OwnershipType;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Tower;
using Util;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

/*
 * This script handles input management for players.
 * It deals with touch input, tower interactions, and player ownership.
 */

namespace Core.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputOwner owner; // Ownership type of the player
        [SerializeField] private LayerMask excludedLayer; // Layer mask to exclude from raycasts
        private TowerBase towerBase; // Reference to the TowerBase currently being interacted with
        private bool isValid; // Flag to check if current tower interaction is valid

        private TowerChangeHandler towerChangeHandler; // Reference to the TowerChangeHandler

        private PhotonView photonView; // Reference to the PhotonView component
        private PlayerUIHandler playerUIHandler; // Reference to the PlayerUIHandler

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            playerUIHandler = GetComponent<PlayerUIHandler>();
            GameManager.Instance.OnGameEnd += OnGameEnd;
            towerChangeHandler = FindFirstObjectByType<TowerChangeHandler>();
            playerUIHandler.SetPlayerName("");
        }

        // Set properties for the input manager based on the index
        public void SetProperties(int index)
        {
            InputOwner clientOwner = GetClientOwnerFromIndex(index - 1);
            photonView.RPC("SpawnInputManager", RpcTarget.AllBuffered, clientOwner);
        }

        // RPC method to spawn input manager with the given owner
        [PunRPC]
        private void SpawnInputManager(InputOwner clientOwner)
        {
            owner = clientOwner;

            // Enable inputs only if this instance is owned by the local player
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(SetNickNameRPC), RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
                EnableInputs();
            }
            else
            {
                DisableInputs();
            }
        }

        // Set player nickname in UI
        public void SetNickName(string name)
        {
            photonView.RPC(nameof(SetNickNameRPC), RpcTarget.All, name);
        }
        [PunRPC]
        private void SetNickNameRPC(string name)
        {
            playerUIHandler.SetPlayerName(name);
        }
        // Map client index to ownership type
        private InputOwner GetClientOwnerFromIndex(int clientIndex)
        {
            switch (clientIndex)
            {
                case 0: return InputOwner.Blue;
                case 1: return InputOwner.Red;
                case 2: return InputOwner.Yellow;
                case 3: return InputOwner.Green;
                default: return InputOwner.Blue;
            }
        }

        // Enable touch inputs
        private void EnableInputs()
        {
            EnhancedTouchSupport.Enable();
            Touch.onFingerDown += Touch_onFingerDown;
            Touch.onFingerMove += Touch_onFingerMove;
            Touch.onFingerUp += Touch_onFingerUp;
        }

        // Disable touch inputs
        private void DisableInputs()
        {
            Touch.onFingerDown -= Touch_onFingerDown;
            Touch.onFingerMove -= Touch_onFingerMove;
            Touch.onFingerUp -= Touch_onFingerUp;
            EnhancedTouchSupport.Disable();
        }

        // Handle cleanup when the game ends
        private void OnGameEnd(bool success)
        {
            if (photonView.IsMine)
            {
                // Perform cleanup logic
            }
        }

        // Handle touch down event
        private void Touch_onFingerDown(Finger finger)
        {
            if (RaycastFromFinger(finger, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out towerBase))
                {
                    if (towerBase.TowerOwner == owner)
                    {
                        if (towerBase.CanCreateConnections)
                        {
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

        // Handle touch move event
        private void Touch_onFingerMove(Finger finger)
        {
            if (towerBase != null && RaycastFromFinger(finger, out RaycastHit hit))
            {
                towerChangeHandler.CloseTowerInventory();
                RaycastHit[] hits = Physics.RaycastAll(towerBase.transform.position, hit.point - towerBase.transform.position, Mathf.Infinity, ~excludedLayer);
                isValid = hits.Length == 1
                    && hits[0].transform.GetComponent<TowerBase>() != null
                    && hit.transform.GetComponent<TowerBase>() != null
                    && hits[0].transform.GetComponent<TowerBase>().TowerID == hit.transform.GetComponent<TowerBase>().TowerID;
                PathManager.Instance.UpdateHintLine(hit.point, isValid);
            }
        }

        // Handle touch up event
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

        // Perform raycast from finger position
        public bool RaycastFromFinger(Finger finger, out RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(finger.screenPosition);
            return Physics.Raycast(ray, out hit, Mathf.Infinity);
        }
    }
}
