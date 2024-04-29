using Util;
using Troop;
using System;
using UnityEngine;
using Core.PathHandler;
using UnityEngine.Events;
using System.Collections.Generic;
using Tier = Core.GameEnums.Tier;
using OwnershipType = Core.GameEnums.OwnershipType;
using TowerType = Core.GameEnums.TowerType;
using ChangeableType = Core.GameEnums.TowerChangeability;
using TMPro;

namespace Tower
{
    public abstract class TowerBase : MonoBehaviour
    {
        [field: SerializeField] public OwnershipType TowerOwner { get; protected set; }

        [field: SerializeField, Disable, BeginGroup("Readonly Settings")] public string TowerID { get; protected set; }
        [field: SerializeField, Disable, EndGroup] public TowerType TowerType { get; protected set; }

        [field: SerializeField, BeginGroup("Tower Change Settings")] public ChangeableType TowerChangeableType { get; protected set; }
        [SerializeField, ShowIf("TowerChangeableType", ChangeableType.Changeable), PrefabObjectOnly, AssetPreview] private GameObject defaultTowerPrefab;
        [SerializeField, ShowIf("TowerChangeableType", ChangeableType.Changeable)] private int minLevelToChange = 5;
        [field: SerializeField, ShowDisabledIf("TowerChangeableType", ChangeableType.Changeable), EndGroup] public bool IsChangeable { get; protected set; }

        [field: SerializeField, ProgressBar("Tower Level", minValue: 0, maxValue: 64, HexColor = "#76ABAE", IsInteractable = true), BeginGroup("Level Settings")] public int Level { get; protected set; }

        [SpaceArea]
        [SerializeField, DisableInPlayMode, BeginHorizontalGroup] protected int maxLevel = 64;

        [field: SerializeField, EndHorizontalGroup, EndGroup] public Tier TowerTier { get; protected set; }

        [SerializeField, BeginGroup("Connetions or Path"), DisableInPlayMode] protected int usedPaths = 0;
        [SerializeField, DisableInPlayMode] protected int maxPaths = 3;
        [field: SerializeField, Disable] public bool CanCreateConnections { get; protected set; }
        [field: SerializeField, LabelByChild("Name"), Disable, EndGroup] public List<TowerConnection> Connections { get; protected set; }

        [field: SerializeField, BeginGroup("Events")] public UnityEvent OnTowerUpgrade_Level { get; protected set; }
        [field: SerializeField] public UnityEvent OnTowerDowngrade_Level { get; protected set; }
        [field: SerializeField] public UnityEvent<OwnershipType> OnTowerOwnerChange { get; protected set; }
        [field: SerializeField, EndGroup] public UnityEvent<Tier, bool> OnTowerTierChanged { get; protected set; }

        [field: SerializeField, BeginGroup("Visual"), LabelByChild("owner")] public OwnerVisual[] Visual { get; protected set; }
        [SerializeField, EndGroup] private TMP_Text levelText;

        protected virtual void Awake()
        {
            Guid guid = Guid.NewGuid();
            TowerID = guid.ToString()[..8];
        }

        protected virtual void Start()
        {
            OnTowerOwnerChange.AddListener((type) => UpdateTowerVisual(type, Tier.Tier1));
            OnTowerOwnerChange?.Invoke(TowerOwner);

            if (Level >= 30)
            {
                TowerTier = Tier.Tier3;
            }
            else if (Level >= 20)
            {
                TowerTier = Tier.Tier2;
            }
            else
            {
                TowerTier = Tier.Tier1;
            }
            OnTowerTierChanged.AddListener((tier, isUpgrading) => UpdateTowerVisual(TowerOwner, tier));
            OnTowerTierChanged?.Invoke(TowerTier, false);
            levelText.text = Level.ToString();
        }

        protected virtual void Update()
        {
            ConnectionCheckUpdate();
            IsChangeable = Level >= minLevelToChange;
            if (Level <= 0)
            {
                Level = 0;
            }
            if (Level >= maxLevel)
            {
                Level = maxLevel;
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

        public void UpdateTowerLevel(TroopBase incomingTroop)
        {
            bool isUpgrading;
            if (incomingTroop.Owner == TowerOwner)
            {
                //! fellow Troop
                isUpgrading = true;
                Level += incomingTroop.CurrentLevel;
                if (Level >= maxLevel)
                {
                    Level = maxLevel;
                    RespawnTroop(incomingTroop);
                }
                OnTowerUpgrade_Level?.Invoke();
            }
            else
            {
                //! !Enemy Attack
                isUpgrading = false;
                Level -= incomingTroop.CurrentLevel;
                OnTowerDowngrade_Level?.Invoke();
                if (Level <= 0)
                {
                    TowerOwner = incomingTroop.Owner;
                    OnTowerOwnerChange?.Invoke(incomingTroop.Owner);
                    if (Connections != null && Connections.Count > 0)
                    {
                        foreach (var con in Connections)
                        {
                            DisconnectTower(con.Tower);
                        }
                    }
                    if (IsChangeable)
                    {
                        //Spawn Default Tower
                    }
                }
            }
            if (Level == 0)
            {
                TowerTier = Tier.Tier1;
                OnTowerTierChanged?.Invoke(Tier.Tier1, isUpgrading);
            }
            else if (Level == 20)
            {
                TowerTier = Tier.Tier2;
                OnTowerTierChanged?.Invoke(Tier.Tier2, isUpgrading);
            }
            else if (Level == 30)
            {
                TowerTier = Tier.Tier3;
                OnTowerTierChanged?.Invoke(Tier.Tier3, isUpgrading);
            }

            levelText.text = Level.ToString();
        }

        public void UpdateTowerLevel(int amt)
        {
            if (amt == 0) return;

            if (amt > 0)
            {
                OnTowerUpgrade_Level?.Invoke();
            }
            else
            {
                OnTowerUpgrade_Level?.Invoke();
            }

            Level += amt;
        }

        private int FindTowerInConnection(TowerBase tower)
        {
            return Connections.FindIndex(connection => connection.Tower.TowerID == tower.TowerID);
        }

        protected virtual void ConnectionCheckUpdate()
        {
            usedPaths = Connections.Count;
            CanCreateConnections = usedPaths < maxPaths;
            maxPaths = (int)TowerTier;
        }

        protected void RespawnTroop(TroopBase troop)
        {
            if (!CanCreateConnections) return;

            int index = UnityEngine.Random.Range(minInclusive: 0, maxExclusive: Connections.Count);
            if (troop is SoldierTroop)
            {
                TroopPooler.Instance.SpawnSoldierTroop(TowerID, Connections[index].Tower.TowerID, TowerOwner, transform.position, Connections[index].Tower.transform.position);
            }
            else if (troop is BruteTroop)
            {
                TroopPooler.Instance.SpawnBruteTroop(TowerID, Connections[index].Tower.TowerID, TowerOwner, transform.position, Connections[index].Tower.transform.position);
            }
        }

        protected abstract void Spawn();

        protected void UpdateTowerVisual(OwnershipType newOwner, Tier tier)
        {
            Vector3 uiPos = Vector3.zero;
            foreach (var visual in Visual)
            {
                if (visual.owner == newOwner)
                {
                    for (int i = 0; i < visual.tierVisuals.Length; i++)
                    {
                        if (tier == visual.tierVisuals[i].towerTier)
                        {
                            visual.tierVisuals[i].TowerLevelObject.SetActive(true);
                            uiPos = visual.tierVisuals[i].TowerLevelObject.transform.GetChild(0).position;
                            //TODO: Set the metarial of active object
                        }
                        else
                        {
                            visual.tierVisuals[i].TowerLevelObject.SetActive(false);
                        }
                    }
                    break;
                }
            }
            levelText.transform.parent.parent.position = uiPos;
        }

        public void CopyTowerSettings(TowerBase tower)
        {
            TowerOwner = tower.TowerOwner;
            Level = tower.Level;
            TowerChangeableType = tower.TowerChangeableType;
            minLevelToChange = tower.minLevelToChange;
        }
    }
}