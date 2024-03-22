using UnityEngine;
using TroopOwner = Core.GameEnums.OwnershipType;

public abstract class TroopBase : MonoBehaviour
{
    [field: SerializeField, Disable] public TroopOwner Owner { get; protected set; }

    [SerializeField] protected int health;
    [SerializeField] protected int level;
}