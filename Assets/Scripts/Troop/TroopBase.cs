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

        [SerializeField] protected int health;
        [field: SerializeField] public int Level { get; protected set; }

        private int currentHealth, currentLevel;

        [SerializeField] private float speed;

        [Space(5f)]
        [SerializeField, NotNull] protected Transform collisionCheckTransform;

        [SerializeField] protected LayerMask collidableLayer;
        [SerializeField] protected float collidableDist;

        private bool isInitialize;

        public virtual void InitTroop(TroopOwner owner, string selfId, string enemyid, Vector3 start, Vector3 end)
        {
            Id = selfId;
            EnemyId = EnemyId;
            Owner = owner;
            currentHealth = health;
            currentLevel = Level;
            transform.position = start;
            transform.forward = (end - start).normalized;
            isInitialize = true;
        }

        protected virtual void Update()
        {
            if (!isInitialize) return;

            if (health <= 0)
            {
                OnDeath();
                isInitialize = false;
                return;
            }
            transform.Translate(transform.forward * speed * Time.deltaTime);
            CollsionCheck();
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
                        if (enemyTroop.EnemyId == EnemyId)
                        {
                            "Enemy Collided".Log(this);
                            DamageToOtherTroop(enemyTroop);
                            break;
                        }
                    }
                    if (col.TryGetComponent<TowerBase>(out var enemyTower))
                    {
                        if (enemyTower.TowerID == EnemyId)
                        {
                            "Tower Collider".Log(this);
                            enemyTower.UpdateTowerLevel(this);
                        }
                    }
                }
            }
        }

        protected abstract void OnDeath();

        protected virtual void DamageToOtherTroop(TroopBase troop)
        {
            troop.currentHealth -= health;
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