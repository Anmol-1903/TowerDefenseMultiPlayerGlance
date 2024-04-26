using System.Collections;
using System.Collections.Generic;
using Networking;
using Photon.Pun;
using Tower;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    [SerializeField] private Transform[] playerSpawnPositions;

    private void Start()
    {
        Spawn();
    }
    private void Spawn()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject player = PhotonNetwork.Instantiate(NetworkManager.Instance.PlayerPrefab.name, playerSpawnPositions[i].position, Quaternion.identity);
            // Assign different colors and enums to players
            PlayerDataContainer playerController = player.GetComponent<PlayerDataContainer>();
            playerController.SetPlayerProperties(NetworkManager.Instance.GameSettings.GetPlayerColors[i], i); // Assuming you have a PlayerController script
        }
    }
}