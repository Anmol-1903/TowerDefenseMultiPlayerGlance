namespace Troop
{
    public class BruteTroop : TroopBase
    {
        protected override void OnDeath()
        {
            TroopPooler.Instance.BrutePool.Release(this);
        }
    }
}