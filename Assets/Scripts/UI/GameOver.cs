using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Owner = Core.GameEnums.OwnershipType;
using UnityEngine.UI;
using System;

public class GameOver : MonoBehaviourPunCallbacks
{
    public static GameOver Instance;
    [SerializeField] private GameObject gameoverCanvas;
    [SerializeField] private GameObject gameWinCanvas;
    [SerializeField] private Button mainMenuButton;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        mainMenuButton.onClick.AddListener(LeaveGame);
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }

    private Owner owner;

    public void SetOwner(Owner own)
    {
        owner = own;
        Tower.TowerTracker.Instance.OnTowerUpdateInScene += CheckGamesStatus;
    }

    public void CheckGamesStatus()
    {
        StartCoroutine(CheckRoutine());
    }

    private IEnumerator CheckRoutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Tower Count = " + Tower.TowerTracker.Instance.TowersByOwner[(int)owner].Towers.Count);
        if (Tower.TowerTracker.Instance.TowersByOwner[(int)owner].Towers.Count <= 0)
        {
            gameoverCanvas.SetActive(true);
           // photonView.RPC("ShowGameOverScreen", RpcTarget.All, PhotonNetwork.LocalPlayer);
        }
        if (Tower.TowerTracker.Instance.TowersByOwner[(int)owner].Towers.Count == Tower.TowerTracker.Instance.TowerList.Count)
        {
            gameWinCanvas.SetActive(true);
           // photonView.RPC("ShowGameWinScreen", RpcTarget.All, PhotonNetwork.LocalPlayer);
        }
    }

    [PunRPC]
    public void ShowGameOverScreen(Photon.Realtime.Player player)
    {
        if (PhotonNetwork.LocalPlayer == player)
        {

            Debug.Log(player.NickName + " Game Over");
            gameoverCanvas.SetActive(true);

        }
    }

    [PunRPC]
    public void ShowGameWinScreen(Photon.Realtime.Player player)
    {
        if (PhotonNetwork.LocalPlayer == player)
        {

            Debug.Log(player.NickName + " You Win");
            gameWinCanvas.SetActive(true);

        }
    }
}