using Core;
using UnityEngine;

namespace Troop
{
    public class BruteTroop : TroopBase
    {
        public override void InitTroop(GameEnums.OwnershipType owner, string selfId, string enemyId, Vector3 start, Vector3 end, TroopDataScriptable troopData)
        {
            base.InitTroop(owner, selfId, enemyId, start, end, troopData);
            currentHealth = data.BruteHealth;
            CurrentLevel = data.BruteLevel;
            isInitialize = true;
        }

        protected override void OnDeath()
        {
            TroopPooler.Instance.BrutePool.Release(this);
        }
    }
}