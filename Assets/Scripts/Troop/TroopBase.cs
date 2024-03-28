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

        [Space(5f)]
        [SerializeField, NotNull] protected Transform collisionCheckTransform;

        [SerializeField] protected LayerMask collidableLayer;
        [SerializeField] protected float collidableDist;

        public virtual void InitTroop(TroopOwner owner, string selfId, string enemyid, Vector3 start, Vector3 end)
        {
            Id = selfId;
            EnemyId = EnemyId;
            Owner = owner;
            transform.position = start;
            transform.forward = (end - start).normalized;
        }

        protected virtual void Update()
        {
            if (health <= 0)
            {
                OnDeath();
                return;
            }

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
                }
            }
        }

        protected abstract void OnDeath();

        protected virtual void DamageToOtherTroop(TroopBase troop)
        {
            troop.health -= health;
        }
    }
}