using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Util;
using TowerType = Core.GameEnums.TowerType;

namespace Tower
{
    public class TowerChangeHandler : MonoBehaviour
    {
        [SerializeField, NotNull] private GameObject holder;
        [SerializeField, LabelByChild("Type")] private TowerInventory[] towerInventoryItems;

        private TowerBase selectedTower;
        private Vector3 towerPosition;

        private void Start()
        {
            UpdateUI();
            holder.SetActive(false);
        }

        public void OpenTowerInventory(TowerBase tower, Vector3 position)
        {
            selectedTower = tower;
            towerPosition = position;
            holder.SetActive(true);
        }

        public void CloseTowerInventory()
        {
            selectedTower = null;
            towerPosition = Vector3.zero;
            holder.SetActive(false);
        }

        public void AddSoldierTower() => ChangeTower(selectedTower.TowerType, TowerType.Soldier, towerPosition);

        public void AddBruteTower() => ChangeTower(selectedTower.TowerType, TowerType.Brute, towerPosition);

        public void AddArcheryTower() => ChangeTower(selectedTower.TowerType, TowerType.Archery, towerPosition);

        private void ChangeTower(TowerType prevType, TowerType nextType, Vector3 towerPosition)
        {
            $"Changing {prevType} to {nextType}".Log();
            int prevIndex = Array.FindIndex(towerInventoryItems, item => prevType == item.Type);
            int nextIndex = Array.FindIndex(towerInventoryItems, item => nextType == item.Type);

            if (towerInventoryItems[nextIndex].NoOfLeft > 0)
            {
                if (prevIndex != -1 && nextIndex != -1)
                {
                    if (prevIndex == nextIndex) // Replacing same tower
                        return;

                    GameObject newTowerObj = Instantiate(towerInventoryItems[nextIndex].Tower.gameObject, towerPosition, Quaternion.identity);
                    TowerBase newTower = newTowerObj.GetComponent<TowerBase>();
                    newTower.CopyTowerSettings(selectedTower);

                    if (towerInventoryItems[prevIndex].IsInfinite == false)
                    {
                        towerInventoryItems[prevIndex].NoOfLeft += 1;
                    }
                    if (towerInventoryItems[nextIndex].IsInfinite == false)
                    {
                        towerInventoryItems[nextIndex].NoOfLeft -= 1;
                    }

                    Destroy(selectedTower.gameObject);
                    //Remeber tower.CopyTowerSettings
                    //Replace it here
                }
                else
                {
                    "Tower is not exist in Tower Inventory".Log(this);
                }
            }
            UpdateUI();
            CloseTowerInventory();
        }

        private void UpdateUI()
        {
            foreach (var item in towerInventoryItems)
            {
                if (item.IsInfinite)
                {
                    item.NoOfLeftText.text = "∞";
                }
                else
                {
                    item.NoOfLeftText.text = item.NoOfLeft.ToString();
                }
            }
        }
    }

    [System.Serializable]
    public struct TowerInventory
    {
        public TowerType Type;
        [Help("Prefab Only"), AssetPreview] public TowerBase Tower;
        public bool IsInfinite;
        [ShowIf("IsInfinite", false)] public int NoOfLeft;
        [InLineEditor] public TMP_Text NoOfLeftText;
    }
}