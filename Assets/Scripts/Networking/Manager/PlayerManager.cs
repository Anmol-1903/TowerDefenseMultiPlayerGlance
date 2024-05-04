using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Input;
using Networking;
using Photon.Pun;
using Tower;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Singleton instance
    public static PlayerManager Instance;

    // Input managers for controlling players
    [SerializeField] private InputManager[] inputManagers;

    // Player spawn positions
    [SerializeField] private Transform[] playerSpawnPositions;

    #region Unity Callbacks

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null) 
            Instance = this;
        else 
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Subscribe to the game start event
        GameManager.Instance.OnGameStart += Spawn;
    }

    // OnDestroy is called when the GameObject is being destroyed
    private void OnDestroy()
    {
        // Unsubscribe from the game start event
        GameManager.Instance.OnGameStart -= Spawn;
    }

    #endregion

    #region Player Spawning

    // Spawns players based on the game settings
    private void Spawn()
    {
        // Loop through input managers
        for (int i = 0; i < inputManagers.Length; i++)
        {
            // Log maximum players in the room
            Debug.Log("Max Players are " + PhotonNetwork.CurrentRoom.MaxPlayers);

            // Activate input manager for each player up to max players
            if (i < PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                inputManagers[i].gameObject.SetActive(true);
                inputManagers[i].SetNickName("Bot");
            }
            else
            {
                // Deactivate input manager for additional players
                inputManagers[i].gameObject.SetActive(false);
            }
        }

        // Set properties for each input manager based on Photon player list
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            inputManagers[i].SetProperties(PhotonNetwork.PlayerList[i].ActorNumber);
        }
    }

    #endregion
}
