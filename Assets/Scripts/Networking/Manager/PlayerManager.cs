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
    public static PlayerManager Instance;
    [SerializeField] private InputManager[] inputManagers;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [SerializeField] private Transform[] playerSpawnPositions;

    private void Start()
    {
        GameManager.Instance.OnGameStart += Spawn;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStart -= Spawn;
    }
    private void Spawn()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            inputManagers[i].SetProperties(PhotonNetwork.PlayerList[i].ActorNumber);
            // PlayerDataContainer playerController = player.GetComponent<PlayerDataContainer>();
            // playerController.SetPlayerProperties(GameManager.Instance.GameSettings.GetPlayerColors[i], i); // Assuming you have a PlayerController script
        }
    }
}