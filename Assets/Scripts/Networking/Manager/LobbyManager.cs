using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Networking
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        private List<Player> players = new List<Player>();

        public override void OnEnable()
        {
            NetworkManager.OnMatchFound += StartGame;
        }

        public override void OnDisable()
        {
            NetworkManager.OnMatchFound -= StartGame;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (!players.Contains(newPlayer))
            {
                players.Add(newPlayer);
                Debug.Log(newPlayer.NickName + " joined the game");
            }
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        }

        private void StartGame(int botCount)
        {
            Debug.Log("Number of bots = " + botCount);
            LoadNextScene();
        }

        private static void LoadNextScene()
        {
            PhotonNetwork.LoadLevel(Core.GameManager.Instance.SceneContainer.TutorialLevelScenes[0]);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (players.Contains(otherPlayer))
            {
                players.Remove(otherPlayer);
                Debug.Log(otherPlayer.NickName + " left the game");
            }
        }
    }
}