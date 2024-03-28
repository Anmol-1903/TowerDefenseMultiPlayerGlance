namespace Troop
{
    public class SoldierTroop : TroopBase
    {
        protected override void OnDeath()
        {
            TroopPooler.Instance.SoldierPool.Release(this);
        }
    }
}