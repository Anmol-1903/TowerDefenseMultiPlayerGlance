using System;
using System.Collections.Generic;
using System.Linq;
using Tower;
using UnityEngine;

using TowerOwner = Core.GameEnums.OwnershipType;
using TowerType = Core.GameEnums.TowerType;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float yOffset;

    [Space(5f)]
    [SerializeField] private TowerOwner owner;

    [SerializeField] private float attackInterval;
    [SerializeField] private LayerMask towerLayer;
    [SerializeField, LabelByChild("PreyOwner")] private Priority[] enemyPriorities;

    private List<TowerBase> selfTower;

    private float nextTimeToAttack;
    private bool canStart;

    public float AttackInterval => attackInterval;
    public Priority[] Priorities => enemyPriorities;

    public bool SetEnemyData(TowerOwner type, float interval, Priority[] priorities)
    {
        owner = type;
        attackInterval = interval;
        enemyPriorities = priorities;

        gameObject.name = $"{type}";
        return true;
    }

    public void InitAI()
    {
        /*        selfTower = new();
                TowerTracker.Instance.OnTowerUpdate += GetSelfTower;
                canStart = true;
                nextTimeToAttack = attackInterval;*/
    }

    private void Update()
    {
        if (canStart)
        {
            if (nextTimeToAttack <= 0)
            {
                nextTimeToAttack = attackInterval;
                Attack();
            }
            else
            {
                nextTimeToAttack -= Time.deltaTime;
            }
        }
        else
        {
            nextTimeToAttack = attackInterval;
        }
    }

    private void OnDestroy()
    {
        /*        TowerTracker.Instance.OnTowerUpdate -= GetSelfTower;
                GameManager.Instance.OnGameStart -= InitAI;*/
    }

    private void Attack()
    {
        foreach (var priority in enemyPriorities)
        {
            for (int i = 0; i < selfTower.Count; i++)
            {
                if (!selfTower[i].CanCreateConnections)
                    continue;

                TowerBase[] possibleTower = GetPossibleTowersToAttack(priority);
                foreach (var tower in possibleTower)
                {
                    Vector3 startpoint = new(selfTower[i].transform.position.x, yOffset, selfTower[i].transform.position.z);
                    Vector3 endpoint = new(tower.transform.position.x, yOffset, tower.transform.position.z);
                    Vector3 dir = endpoint - startpoint;

                    if (TryToConnect(startpoint, dir, priority, selfTower[i])) //! Try to connect with prey and if conected exit the method
                    {
                        return;
                    }
                }
            }
        }
    }

    private bool TryToConnect(Vector3 startpoint, Vector3 dir, Priority priority, TowerBase selfTower)
    {
        if (Physics.Raycast(startpoint, dir.normalized, out RaycastHit hit, dir.magnitude, towerLayer))
        {
            if (hit.collider.CompareTag("Tower"))
            {
                if (hit.collider.TryGetComponent<TowerBase>(out var preyTower))
                {
                    if (priority.FilterByLevel && selfTower.Level < preyTower.Level)
                    {
                        return false;
                    }

                    if (selfTower.ConnectTo(preyTower) != null)//! if successfully connected return true
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private TowerBase[] GetPossibleTowersToAttack(Priority priority)
    {
        TowerBase[] preyByOwner, preyByType;
        preyByOwner = TowerTracker.Instance.GetTowerByOwner(priority.PreyOwner).Except(selfTower).ToArray();
        preyByType = TowerTracker.Instance.GetTowerByType(priority.PreyTowerType).ToArray();

        return preyByOwner.Intersect(preyByType).ToArray();
    }

    private void GetSelfTower()
    {
        //Get All his tower
    }
}

[Serializable]
public struct Priority
{
    public bool FilterByLevel;
    public TowerType PreyTowerType;
    public TowerOwner PreyOwner;
}