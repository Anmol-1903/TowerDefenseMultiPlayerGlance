using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Owner = Core.GameEnums.OwnershipType;

public class GameOver : MonoBehaviour
{
    private Owner owner;

    public void SetOwner(Owner own)
    {
        owner = own;
        Tower.TowerTracker.Instance.OnTowerUpdateInScene += CheckGamesStatus;
    }

    public void CheckGamesStatus()
    {
        if (Tower.TowerTracker.Instance.TowersByOwner[(int)owner].Towers.Count <= 0)
        {
            //Show Game Over
        }
        if (Tower.TowerTracker.Instance.TowersByOwner[(int)owner].Towers.Count == Tower.TowerTracker.Instance.TowerList.Count)
        {
            // won the match
        }
    }
}