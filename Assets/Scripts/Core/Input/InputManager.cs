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

        private void Start()
        {
            GameManager.Instance.OnGameStart += EnableInputs;
            GameManager.Instance.OnGameEnd += OnGameEnd;
            towerChangeHandler = FindFirstObjectByType<TowerChangeHandler>();
            //playerUIHandler.SetPlayerName("");
        }

        // Set properties for the input manager based on the index
        public void SetOwner(int index)
        {
            owner = GetClientOwnerFromIndex(index - 1);
            owner.Log();
        }

        // Map client index to ownership type
        private InputOwner GetClientOwnerFromIndex(int clientIndex)
        {
            return clientIndex switch
            {
                0 => InputOwner.Blue,
                1 => InputOwner.Red,
                2 => InputOwner.Yellow,
                3 => InputOwner.Green,
                _ => InputOwner.Blue,
            };
        }

        // Enable touch inputs
        private void EnableInputs()
        {
            TouchSimulation.Enable();
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
            TouchSimulation.Disable();
        }

        // Handle cleanup when the game ends
        private void OnGameEnd(bool success)
        {
            DisableInputs();
        }

        // Handle touch down event
        private void Touch_onFingerDown(Finger finger)
        {
            if (RaycastFromFinger(finger, out RaycastHit hit))
            {
                if (hit.collider.transform.root.TryGetComponent(out towerBase))
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
                    && hits[0].transform.root.GetComponent<TowerBase>() != null
                    && hit.transform.root.GetComponent<TowerBase>() != null
                    && hits[0].transform.root.GetComponent<TowerBase>().TowerID == hit.transform.transform.root.GetComponent<TowerBase>().TowerID;
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
                        towerBase.ConnectTo(hit.transform.root.GetComponent<TowerBase>());
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