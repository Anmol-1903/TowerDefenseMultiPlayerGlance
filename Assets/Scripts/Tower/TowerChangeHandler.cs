using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using TowerType = Core.GameEnums.TowerType;

namespace Tower
{
    public class TowerChangeHandler : MonoBehaviour
    {
        [SerializeField, LabelByChild("Type")] private TowerInventory[] towerInventoryItems;

        private GameObject holder;

        private TowerBase selectedTower;
        private Vector3 towerPosition;

        private void Start()
        {
            holder = transform.GetChild(0).gameObject;
            holder.SetActive(false);
        }

        public void OpenTowerInventory()
        {
            holder.SetActive(true);
        }

        public void CloseTowerInventory()
        {
            holder.SetActive(false);
        }

        public void SelectTower(TowerBase tower, Vector3 position)
        {
            selectedTower = tower;
            towerPosition = position;
        }

        //Button Methond
        public void AddTower(TowerType type)
        {
            ChangeTower(selectedTower.TowerType, type, towerPosition);
        }

        private void ChangeTower(TowerType prevType, TowerType nextType, Vector3 towerPosition)
        {
            int prevIndex = Array.FindIndex(towerInventoryItems, item => prevType == item.Type);
            int nextIndex = Array.FindIndex(towerInventoryItems, item => nextType == item.Type);

            if (prevIndex != -1 && nextIndex != -1)
            {
                if (prevIndex == nextIndex) // Replacing same tower
                    return;

                //Remeber tower.CopyTowerSettings
                //Replace it here
            }
            else
            {
                "Tower is not exist in Tower Inventory".Log(this);
            }
        }
    }

    [System.Serializable]
    public struct TowerInventory
    {
        public TowerType Type;
        [Help("Prefab Only"), AssetPreview] public TowerBase Tower;
        public bool IsInfinite;
        [ShowIf("IsInfinite", false)] public string NoOfLeft;
    }
}