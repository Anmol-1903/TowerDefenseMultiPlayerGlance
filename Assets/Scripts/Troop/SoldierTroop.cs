using Core;
using UnityEngine;
using Photon.Pun;

namespace Troop
{
    public class SoldierTroop : TroopBase
    {
        [HideInInspector] public PhotonView pv;

        public override void InitTroop(GameEnums.OwnershipType owner, string selfId, string enemyId, Vector3 start, Vector3 end, TroopDataScriptable troopData)
        {
            //pv = GetComponent<PhotonView>();
            //pv.RPC("InitTroopRPC", RpcTarget.All, owner,  selfId,  enemyId,  start,  end,  troopData);
            base.InitTroop(owner, selfId, enemyId, start, end, troopData);
            currentHealth = data.SoldierHealth;
            CurrentLevel = data.SoldierLevel;
            isInitialize = true;
        }

        [PunRPC]
        private void InitTroopRPC(GameEnums.OwnershipType owner, string selfId, string enemyId, Vector3 start, Vector3 end, TroopDataScriptable troopData)
        {
            base.InitTroop(owner, selfId, enemyId, start, end, troopData);
            currentHealth = data.SoldierHealth;
            CurrentLevel = data.SoldierLevel;
            isInitialize = true;
        }

        [PunRPC]
        private void OnDeathRPC()
        {
        }

        protected override void OnDeath()
        {
            TroopPooler.Instance.SoldierPool.Release(this);
            //pv.RPC("OnDeathRPC", RpcTarget.All);
        }
    }
}