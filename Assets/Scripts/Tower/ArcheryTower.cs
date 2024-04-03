using UnityEngine;
using System.Collections;
using Util;
using TowerType = Core.GameEnums.TowerType;
using OwnerShipType = Core.GameEnums.OwnershipType;
using UnityEngine.Rendering;
using Core;
using Troop;

namespace Tower
{
    public class ArcheryTower : TowerBase
    {
        [SerializeField] private float attackInterval, _attackingRange;
        [SerializeField] float _InitialVel, _Angle;
        [SerializeField] GameObject _test;
        [SerializeField] LayerMask _troopsLayer;
        [SerializeField] LeanTweenType _leanTweenType;
        private float currentAttackInterval;

        protected override void Awake()
        {
            base.Awake();
            TowerType = TowerType.Archery;
            CanCreateConnections = false;
            usedPaths = maxPaths = 0;
        }

        protected override void Update()
        {
            {
                
            }
            base.Update();
            if (currentAttackInterval < 0)
            {
                Spawn();
                currentAttackInterval = attackInterval;
            }
            else
            {
                currentAttackInterval -= Time.deltaTime;
            }
        }

        protected override void ConnectionCheckUpdate()
        {
            return;
        }

        protected override void Spawn()
        {
            //spawn attack/projectile
            Collider[] troopCol = Physics.OverlapSphere(transform.position, _attackingRange , _troopsLayer);
            foreach (Collider col in troopCol)
            {
                if (col.gameObject.GetComponent<Collider>().TryGetComponent<TroopBase>(out TroopBase otherOwner))
                {
                    if (TowerOwner != otherOwner.Owner)
                    {
                        Debug.Log("Hello " + col.gameObject.name);
                        LeanTween.move(_test, col.transform, 1f).setEase(_leanTweenType).setOnUpdate((float f) => { "Wow".Log(); }) ;
                    }
                    else
                    {
                        "This is my troop".Log(this);
                    }
                    
                }
            }

        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _attackingRange);
        }
    }
}