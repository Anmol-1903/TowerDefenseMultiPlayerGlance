using Core;
using UnityEngine;
using Photon.Pun;

namespace Troop
{
    public class BruteTroop : TroopBase
    {
        [HideInInspector] public PhotonView pv;

        public override void InitTroop(GameEnums.OwnershipType owner, string selfId, string enemyId, Vector3 start, Vector3 end, TroopDataScriptable troopData)
        {
            base.InitTroop(owner, selfId, enemyId, start, end, troopData);
            currentHealth = data.BruteHealth;
            CurrentLevel = data.BruteLevel;
            isInitialize = true;
            //pv = GetComponent<PhotonView>();
        }

        [PunRPC]
        private void OnDeathRPC()
        {
        }

        protected override void OnDeath()
        {
            TroopPooler.Instance.BrutePool.Release(this);
            //pv.RPC("OnDeathRPC", RpcTarget.All);
        }
    }
}