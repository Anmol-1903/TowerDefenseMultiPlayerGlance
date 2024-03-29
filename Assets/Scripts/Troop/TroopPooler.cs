using UnityEngine;
using UnityEngine.Pool;
using UnitySingleton;

namespace Troop
{
    public class TroopPooler : MonoSingleton<TroopPooler>
    {
        [SerializeField, PrefabObjectOnly, NotNull, AssetPreview] private SoldierTroop soldierPrefab;
        [SerializeField, PrefabObjectOnly, NotNull, AssetPreview] private BruteTroop brutePrefab;
        [SerializeField] private int poolSize, maxSize;

        public IObjectPool<SoldierTroop> SoldierPool { get; private set; }
        public IObjectPool<BruteTroop> BrutePool { get; private set; }

        protected override void OnInitialized()
        {
            SoldierPool = new ObjectPool<SoldierTroop>(CreateSoldierPool, OnGetSoldier, OnReleaseSoldier, OnDestroySoldier, false, poolSize, maxSize);
            BrutePool = new ObjectPool<BruteTroop>(CreateBrutePool, OnGetBrute, OnReleaseBrute, OnDestroyBrute, false, poolSize, maxSize);
        }

        public override void ClearSingleton()
        {
            SoldierPool.Clear();
        }

        public void SpawnSoldierTroop(string selfId, string enemyId, Core.GameEnums.OwnershipType owner, Vector3 startPoint, Vector3 endPoint)
        {
            var soldier = SoldierPool.Get();
            soldier.InitTroop(owner, selfId, enemyId, startPoint, endPoint);
        }

        public void SpawnBruteTroop(string selfId, string enemyId, Core.GameEnums.OwnershipType owner, Vector3 startPoint, Vector3 endPoint)
        {
            var brute = BrutePool.Get();
            brute.InitTroop(owner, selfId, enemyId, startPoint, endPoint);
        }

        private SoldierTroop CreateSoldierPool()
        {
            return Instantiate(soldierPrefab);
        }

        private void OnGetSoldier(SoldierTroop obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnReleaseSoldier(SoldierTroop obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestroySoldier(SoldierTroop obj)
        {
            Destroy(obj);
        }

        private BruteTroop CreateBrutePool()
        {
            return Instantiate(brutePrefab);
        }

        private void OnGetBrute(BruteTroop obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnReleaseBrute(BruteTroop obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestroyBrute(BruteTroop obj)
        {
            Destroy(obj);
        }
    }
}