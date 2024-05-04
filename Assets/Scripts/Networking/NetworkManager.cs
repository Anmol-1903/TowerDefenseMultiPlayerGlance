using System.Collections;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;
using Util;
using Core;
using Random = UnityEngine.Random;
namespace Networking
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        #region Initialization

        private static NetworkManager instance;

        public static NetworkManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<NetworkManager>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(NetworkManager).Name;
                        instance = obj.AddComponent<NetworkManager>();
                    }
                }
                return instance;
            }
        }

        private GameSettings gameSettings;

        [HideInInspector] public bool IsConnected = false;

        //do we need them?
        [SerializeField] private float timerDuration = 15f; // Duration of the timer in seconds

        [SerializeField] private TextMeshProUGUI timerText; // just remove it

        [SerializeField] private GameObject playerPrefab; public GameObject PlayerPrefab
        { get { return playerPrefab; } }

        private bool timerStarted = false;
        private Coroutine timerCoroutine;
        private int maxPlayers;
        public static Action<int> OnMatchFound = delegate { }; //param1 for number of bots

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                // Destory duplicates
                if (Application.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
            }

            // gameSettings = GameManager.Instance.GameSettings;
            //timerText.text = "";
        }

        private void Start()
        {
            if (GameManager.Instance.GameSettings != null) gameSettings = GameManager.Instance.GameSettings;
        }
        public static void CreateInstance()
        {
            DestroyInstance();
            instance = Instance;
        }

        public static void DestroyInstance()
        {
            if (instance == null)
            {
                return;
            }
            instance = default;
        }

        public void InitializePhoton(GameSettings _gameSettings)
        {
            gameSettings = _gameSettings;
            Debug.Log("Connecting To Server");
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }

        #endregion Initialization

        #region Connection Callbacks

        public override void OnConnectedToMaster()
        {
            // Called when connected to the Photon server
            PhotonNetwork.NickName = gameSettings.GetNickName;
            PhotonNetwork.GameVersion = gameSettings.GameVersion;
            IsConnected = true;
            Debug.Log("Connected To Server with nickname " + PhotonNetwork.LocalPlayer.NickName);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            // Called when disconnected from the Photon server
            IsConnected = false;
            Debug.Log("You were disconnected due to : " + cause);
        }

        #endregion Connection Callbacks

        #region Room Management

        public void JoinOrCreateRoom()
        {
            maxPlayers = Random.Range(2, gameSettings.MaxPlayers + 1);
            if (!PhotonNetwork.IsConnected) return;
            // Attempt to join or create a room if connected
            if (PhotonNetwork.IsConnected)
            {
                // Try to join an existing room
                RoomOptions roomOptions = new RoomOptions
                {
                    MaxPlayers = maxPlayers
                };

                //TODO: Need to way to join random room and if failed one then create new one
                PhotonNetwork.JoinOrCreateRoom("MyRoom", roomOptions, TypedLobby.Default);
                StartTimer();
            }
        }

        public override void OnJoinedRoom()
        {
            // Called when successfully joined a room
            Debug.Log("Total Players = " + PhotonNetwork.CurrentRoom.MaxPlayers);
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

        #endregion Room Management

        private void StartTimer()
        {
            //todo Use Countdown Helper Coroutine
            StartCoroutine(HelperCoroutine.Countdown(15,
                onTimerUpdate: (float val) =>
                {
                    if (timerText != null)
                    {
                        int timer = (int)val;
                        timerText.text = timer.ToString();
                    }
                    //val.Log(this);
                }, // pass the yield break to break the couroutine in onTimerUpdate if needed
                onComplete: () =>
                {
                    int remainingSlots = 0;
                    if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayers) 
                    {
                        remainingSlots = 0;
                        // If player count reaches max within timer duration, stop the timer
                    }
                    else if (PhotonNetwork.CurrentRoom.PlayerCount < maxPlayers)
                    {
                        remainingSlots = maxPlayers - PhotonNetwork.CurrentRoom.PlayerCount;
                        OnMatchFound?.Invoke(remainingSlots);
                        $"Going with {remainingSlots} bots".Log();
                    }
                    LobbyManager.StartGame(remainingSlots);
                    // OnMatchFound?.Invoke(remainingSlots);
                } // what to do when timer is completed
              ));
        }
    }
}