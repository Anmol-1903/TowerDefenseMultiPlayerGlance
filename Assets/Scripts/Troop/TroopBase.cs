using UnityEngine;
using TroopOwner = Core.GameEnums.OwnershipType;

namespace Troop
{
    public abstract class TroopBase : MonoBehaviour
    {
        [field: SerializeField, Disable] public string id { get; protected set; }
        [field: SerializeField, Disable] public string enemyId { get; protected set; }

        [field: SerializeField, Disable] public TroopOwner Owner { get; protected set; }

        [SerializeField] protected int health;
        [field: SerializeField] public int Level { get; protected set; }

        protected virtual void Update()
        {
        }
    }
}