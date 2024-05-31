using Photon.Pun;
using System.Collections;
using Troop;
using UI;
using UnityEngine;
using TowerType = Core.GameEnums.TowerType;

namespace Tower
{
    public class SoldierTower : TowerBase
    {
        [SerializeField] private float soldierSpawnInterval;

        private float currentSpawnRate;

        private PhotonView photonview;

        protected override void Awake()
        {
            base.Awake();
            TowerType = TowerType.Soldier;
        }

        protected override void Start()
        {
            base.Start();
            currentSpawnRate = soldierSpawnInterval;
            photonview = GetComponent<PhotonView>();

            StartCoroutine(UpdateLevelData());
        }

        protected override void Spawn()
        {
            if (Connections.Count > 0)
            {
                foreach (var connection in Connections)
                {
                    if (connection.Tower == null)
                        return;
                    TroopPooler.Instance.SpawnSoldierTroop(TowerID, connection.Tower.TowerID, TowerOwner, transform.position, connection.Tower.transform.position);
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            if (currentSpawnRate < 0)
            {
                Spawn();
                currentSpawnRate = soldierSpawnInterval;
            }
            else
            {
                currentSpawnRate -= Time.deltaTime;
            }
        }

        /// <summary>
        /// Base Testing code for syncing
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateLevelData()
        {
            //  Debug.Log(" UpdateLevelData = " + PhotonNetwork.IsConnected);
            Debug.Log(" [BaseTest] Get Towerower type" + TowerOwner);
            while (PhotonNetwork.IsConnected)
            {
                yield return new WaitForSeconds(.1f);
                UPdateTowerLevel();
            }
        }

        [PunRPC]
        public void SendTowerValue(int currentlevel, PhotonMessageInfo info)
        {
            // Debug.Log(info.Sender + ", CurrentLevel: " + currentlevel + ", SenderphotonID " + info.photonView.ViewID + "  ,My photon ID" + photonview.ViewID);
            if (photonview.IsMine)
            {
                if (info.photonView.ViewID == GetComponent<PhotonView>().ViewID)
                {
                    Level = currentlevel;
                }
            }
        }

        public void UPdateTowerLevel()
        {
            this.gameObject.GetComponent<PhotonView>().RPC("SendTowerValue", RpcTarget.All, Level);
        }
    }
}