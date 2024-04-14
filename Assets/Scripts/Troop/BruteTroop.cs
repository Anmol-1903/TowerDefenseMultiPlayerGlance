using Core;

namespace Troop
{
    public class BruteTroop : TroopBase
    {
        public override void InitTroop(GameEnums.OwnershipType owner, string selfId, string enemyId, Vector3 start, Vector3 end, TroopDataScriptable troopData)
        {
            base.InitTroop(owner, selfId, enemyId, start, end, troopData);
            currentHealth = data.BruteHealth;
            currentLevel = data.BruteLevel;
        }

        protected override void OnDeath()
        {
            TroopPooler.Instance.BrutePool.Release(this);
        }
    }
}