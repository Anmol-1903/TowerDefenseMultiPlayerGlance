using UnityEngine;
using TroopOwner = Core.GameEnums.OwnershipType;

public abstract class TroopBase : MonoBehaviour
{
    [field: SerializeField, Disable] public TroopOwner Owner { get; protected set; }

    [SerializeField] protected int health;
    [field: SerializeField] public int Level { get; protected set; }
}