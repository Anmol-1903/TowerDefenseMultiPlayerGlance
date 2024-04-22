using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Initialization
    public static NetworkManager Instance;
    [SerializeField] private GameSettings gameSettings;
    public GameSettings GameSettings { get { return gameSettings; } }
    [HideInInspector] public bool IsConnected = false;
    [SerializeField] float timerDuration = 15f; // Duration of the timer in seconds
    [SerializeField] private TextMeshProUGUI timerText;
    private bool timerStarted = false;
    private Coroutine timerCoroutine;

    public static Action<int> OnGameStarted = delegate { }; //param1 for number of bots
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        timerText.text = "";
    }
    public void InitializePhoton()
    {
        Debug.Log("Connecting To Server");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    #region Connection Callbacks

    public override void OnConnectedToMaster()
    {
        // Called when connected to the Photon server
        PhotonNetwork.NickName = gameSettings.GetNickName;
        PhotonNetwork.GameVersion = gameSettings.GetGameVersion;
        IsConnected = true;
        Debug.Log("Connected To Server with nickname " + PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // Called when disconnected from the Photon server
        IsConnected = false;
        Debug.Log("You were disconnected due to : " + cause);
    }

    #endregion

    #region Room Management

    public void JoinOrCreateRoom()
    {
        if (!PhotonNetwork.IsConnected) return;
        // Attempt to join or create a room if connected
        if (PhotonNetwork.IsConnected)
        {
            // Try to join an existing room
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = gameSettings.MaxPlayers
            };
            PhotonNetwork.JoinOrCreateRoom("MyRoom", roomOptions, TypedLobby.Default);
            StartTimer();
        }
    }

    public override void OnJoinedRoom()
    {
        // Called when successfully joined a room
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // Called when failed to join a room, creating a new room
        Debug.Log("Failed to join room. Creating a new room.");

        RoomOptions roomOptions = new RoomOptions();
        PhotonNetwork.CreateRoom("MyRoom", roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        // Called when a new room is successfully created
        Debug.Log("Room created: " + PhotonNetwork.CurrentRoom.Name);
    }

    #endregion

    private void StartTimer()
    {
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }
    private IEnumerator TimerCoroutine()
    {
        timerStarted = true;
        float timer = timerDuration;

        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;

            if (PhotonNetwork.CurrentRoom.PlayerCount == gameSettings.MaxPlayers)
            {
                // If player count reaches max within timer duration, stop the timer
                timerStarted = false;
                OnGameStarted?.Invoke(0);
                yield break;
            }
            if (timerText != null)
            {
                timerText.text = timer.ToString();
            }
        }

        // If player count does not reach max within timer duration, fill remaining positions with bots
        if (PhotonNetwork.CurrentRoom.PlayerCount < gameSettings.MaxPlayers)
        {
            int remainingSlots = gameSettings.MaxPlayers - PhotonNetwork.CurrentRoom.PlayerCount;
            OnGameStarted?.Invoke(remainingSlots);
        }
    }
}
