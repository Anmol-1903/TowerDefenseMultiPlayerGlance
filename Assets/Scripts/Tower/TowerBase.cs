using UnityEngine;
using Core;
using System.Collections.Generic;
using Util;
using UnityEditor.MemoryProfiler;
using UnityEngine.Events;
using Core.PathHandler;

namespace Tower
{
    public abstract class TowerBase : MonoBehaviour
    {
        [field: SerializeField] public GameEnums.OwnershipType TowerOwner { get; protected set; }

        [field: SerializeField, Disable, BeginGroup("Readonly Settings")] public string TowerID { get; protected set; }
        [field: SerializeField, Disable, EndGroup] public GameEnums.TowerType TowerType { get; protected set; }

        [field: SerializeField, ProgressBar("Tower Level", minValue: 0, maxValue: 64, HexColor = "#76ABAE", IsInteractable = true), BeginGroup("Level Settings")] public int Level { get; protected set; }

        [SpaceArea]
        [SerializeField, DisableInPlayMode, BeginHorizontalGroup(labelToWidthRatio: 0.2f),] protected int maxLevel = 64;

        [field: SerializeField, EndHorizontalGroup, EndGroup] public GameEnums.Tier TowerTier { get; protected set; }

        [SerializeField, BeginGroup("Connetions or Path"), DisableInPlayMode] protected int usedPaths = 0;
        [SerializeField, DisableInPlayMode] protected int maxPaths = 3;
        [field: SerializeField, Disable] public bool CanCreateConnections { get; protected set; }
        [field: SerializeField, LabelByChild("Name"), Disable, EndGroup] public List<TowerConnection> Connections { get; protected set; }

        [field: SerializeField, BeginGroup("Events")] public UnityEvent OnTowerUpgrade_Level { get; protected set; }
        [field: SerializeField] public UnityEvent OnTowerDowngrade_Level { get; protected set; }
        [field: SerializeField, EndGroup] public UnityEvent<GameEnums.Tier> OnTowerTierChanged { get; protected set; }

        protected virtual void Awake()
        {
            System.Guid guid = System.Guid.NewGuid();
            TowerID = guid.ToString()[..8];
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
            ConnectionCheckUpdate();
            if (Level <= 0)
            {
                Level = 0;
            }
        }

        public Path ConnectTo(TowerBase tower)
        {
            if (CanCreateConnections)
            {
                if (FindTowerInConnection(tower) != -1) //! Tower already connected
                    return null;

                //! Tower is not connected yet
                Path path = PathManager.Instance.CreatePath(transform.position, tower.transform.position, this);
                Connections.Add(new(tower, path));
                if (tower.TowerOwner == TowerOwner)
                {
                    int selfIndexInOtherTower = tower.FindTowerInConnection(this);
                    if (selfIndexInOtherTower != -1)
                    {
                        tower.DisconnectTower(tower.Connections[selfIndexInOtherTower].Tower);
                    }
                }
                return path;
            }
            return null;
        }

        public bool DisconnectTower(TowerBase tower)
        {
            bool isDisconnected = false;
            int index = FindTowerInConnection(tower);
            if (index != -1)
            {
                Path path = Connections[index].TowerPath;
                Connections.RemoveAt(index);
                PathManager.Instance.RemovePath(path);
            }

            return isDisconnected;
        }

        private int FindTowerInConnection(TowerBase tower)
        {
            return Connections.FindIndex(connection => connection.Tower.TowerID == tower.TowerID);
        }

        protected virtual void ConnectionCheckUpdate()
        {
            CanCreateConnections = usedPaths < maxPaths;
            maxPaths = (int)TowerTier;
        }
    }
}