using Tower;
using UnityEngine;
using Util;
using TroopOwner = Core.GameEnums.OwnershipType;

namespace Troop
{
    public abstract class TroopBase : MonoBehaviour
    {
        [field: SerializeField, Disable] public string Id { get; protected set; }
        [field: SerializeField, Disable] public string EnemyId { get; protected set; }

        [field: SerializeField, Disable] public TroopOwner Owner { get; protected set; }

        [field: SerializeField, Disable] public int CurrentLevel { get; protected set; }

        protected int currentHealth;
        protected TroopDataScriptable data;

        [SerializeField] private float speed;

        [Space(5f)]
        [SerializeField, NotNull] protected Transform collisionCheckTransform;

        [SerializeField] protected LayerMask collidableLayer;
        [SerializeField] protected float collidableDist;

        private Vector3 startPos, endPos;

        protected bool isInitialize;

        public virtual void InitTroop(TroopOwner owner, string selfId, string enemyId, Vector3 start, Vector3 end, TroopDataScriptable troopData)
        {
            data = troopData;
            Id = selfId;
            EnemyId = enemyId;
            Owner = owner;
            startPos = start;
            endPos = end;
            transform.position = start;
            transform.forward = (end - start).normalized;
        }

        protected virtual void Update()
        {
            if (!isInitialize) return;

            if (currentHealth <= 0)
            {
                OnDeath();
                isInitialize = false;
                return;
            }
            CollsionCheck();
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }

        private void CollsionCheck()
        {
            Collider[] cols = Physics.OverlapSphere(collisionCheckTransform.position, collidableDist, collidableLayer);
            if (cols != null)
            {
                foreach (Collider col in cols)
                {
                    if (col.TryGetComponent<TroopBase>(out var enemyTroop))
                    {
                        if (enemyTroop.Id == EnemyId)
                        {
                            "Enemy Collided".Log(Color.red, this);
                            DamageToOtherTroop(enemyTroop);
                            break;
                        }
                    }
                    if (col.transform.root.TryGetComponent<TowerBase>(out var enemyTower))
                    {
                        if (enemyTower.TowerID == EnemyId)
                        {
                            "Tower Collider".Log(Color.blue, this);
                            enemyTower.UpdateTowerLevel(this);
                            currentHealth = 0;
                        }
                    }
                }
            }
        }

        protected abstract void OnDeath();

        protected virtual void DamageToOtherTroop(TroopBase troop)
        {
            troop.currentHealth -= CurrentLevel;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(collisionCheckTransform.position, collidableDist);
        }

#endif
    }
}