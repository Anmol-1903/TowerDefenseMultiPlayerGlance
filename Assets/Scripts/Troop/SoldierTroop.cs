using Core;
using UnityEngine;
using Photon.Pun;

namespace Troop
{
    public class SoldierTroop : TroopBase
    {
        PhotonView pv;
        public override void InitTroop(GameEnums.OwnershipType owner, string selfId, string enemyId, Vector3 start, Vector3 end, TroopDataScriptable troopData)
        {
            base.InitTroop(owner, selfId, enemyId, start, end, troopData);
            currentHealth = data.SoldierHealth;
            CurrentLevel = data.SoldierLevel;
            pv = GetComponent<PhotonView>();
        }
        [PunRPC]
        void OnDeathRPC()
        {
            TroopPooler.Instance.SoldierPool.Release(this);
        }
        protected override void OnDeath()
        {
            pv.RPC("OnDeathRPC", RpcTarget.All);
        }
    }
}