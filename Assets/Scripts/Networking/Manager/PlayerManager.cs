using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core;
using Core.Input;
using Networking;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Tower;
using UnityEngine;
using Util;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerUIHandler handler;

    // Input managers for controlling players
    [SerializeField] private InputManager inputManager;

    private PhotonView view;

    #region Unity Callbacks

    // Start is called before the first frame update
    private void Start()
    {
        view = GetComponent<PhotonView>();
        // Subscribe to the game start event
        //GameManager.Instance.OnGameStart += Spawn;
        //PhotonNetwork.Instantiate(Path.Combine("Prefabs", "InputManager"), Vector3.zero, Quaternion.identity);
        Instantiate(inputManager.gameObject).GetComponent<InputManager>().SetOwner(PhotonNetwork.LocalPlayer.ActorNumber);
        if (PhotonNetwork.IsMasterClient)
        {
            view.RPC(nameof(Countdown), RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Countdown()
    {
        StartCoroutine(HelperCoroutine.Countdown(3,
         onTimerUpdate: (val) =>
         {
             //SomeTextEffect?
             handler.SetPlayerName(val.ToString("0"));
         }, onComplete: () =>
         {
             "GameStart!".Log();
             handler.SetPlayerName(PhotonNetwork.NickName);
             Core.GameManager.Instance.OnGameStart?.Invoke();
         }));
    }

    // OnDestroy is called when the GameObject is being destroyed
    private void OnDestroy()
    {
        // Unsubscribe from the game start event
        //GameManager.Instance.OnGameStart -= Spawn;
    }

    #endregion Unity Callbacks

    #region Player Spawning

    // Spawns players based on the game settings
    private void Spawn()
    {
        // Loop through input managers
        /*        for (int i = 0; i < inputManagers.Length; i++)
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
                }*/

        // Set properties for each input manager based on Photon player list
        /*        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    inputManager[i].SetProperties(PhotonNetwork.PlayerList[i].ActorNumber);
                }*/
    }

    #endregion Player Spawning
}