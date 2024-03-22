using UnityEngine;
using Core;
using System.Collections.Generic;
using Util;

namespace Tower
{
    public abstract class TowerBase : MonoBehaviour
    {
        [field: SerializeField] public GameEnums.PlayerType TowerOwner { get; protected set; }

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

        protected virtual void ConnectionCheckUpdate()
        {
            CanCreateConnections = usedPaths < maxPaths;
            maxPaths = (int)TowerTier;
            "Checking Connection".Log(this);
        }
    }
}