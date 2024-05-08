using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Pool;
using UnitySingleton;

namespace Troop
{
    public class TroopPooler : MonoSingleton<TroopPooler>
    {

        [SerializeField, Disable] private TroopDataScriptable troopData;
        public IObjectPool<SoldierTroop> SoldierPool { get; private set; }
        public IObjectPool<BruteTroop> BrutePool { get; private set; }

        public IEnumerator GetTroopPoolData()
        {
            yield return StartCoroutine(Util.HelperCoroutine.LoadDataFromResources("Scriptable/TroopData", (data) =>
            {
                troopData = data as TroopDataScriptable;
                Init();
            }));
        }

        private void Init()
        {
            SoldierPool = new ObjectPool<SoldierTroop>(CreateSoldierPool, OnGetSoldier, OnReleaseSoldier, OnDestroySoldier, false, troopData.SoldierPoolSize, troopData.SoldierMaxPoolSize);
            BrutePool = new ObjectPool<BruteTroop>(CreateBrutePool, OnGetBrute, OnReleaseBrute, OnDestroyBrute, false, troopData.BrutePoolSize, troopData.BruteMaxPoolSize);
        }

        public override void ClearSingleton()
        {
            SoldierPool.Clear();
        }
        public void SpawnSoldierTroop(string selfId, string enemyId, Core.GameEnums.OwnershipType owner, Vector3 startPoint, Vector3 endPoint)
        {
            SoldierTroop soldier = SoldierPool.Get();
            soldier.InitTroop(owner, selfId, enemyId, startPoint, endPoint, troopData);
        }

        public void SpawnBruteTroop(string selfId, string enemyId, Core.GameEnums.OwnershipType owner, Vector3 startPoint, Vector3 endPoint)
        {
            BruteTroop brute = BrutePool.Get();
            brute.InitTroop(owner, selfId, enemyId, startPoint, endPoint, troopData);
        }

        private SoldierTroop CreateSoldierPool()
        {
            GameObject troop = PhotonNetwork.Instantiate("SoldierTroop", Vector3.zero, Quaternion.identity);
            return troop.GetComponent<SoldierTroop>();
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
            return Instantiate(troopData.BrutePrefab);
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