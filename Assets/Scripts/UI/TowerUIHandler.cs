using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tower;
using UnityEngine;

namespace UI
{
    public class TowerUIHandler : MonoBehaviour
    {
        private Camera gameCamera;
        private TowerBase tower;
        [SerializeField] private TMP_Text levelText;

        private void Start()
        {
            gameCamera = Camera.main;
            transform.GetComponentInChildren<Canvas>().worldCamera = gameCamera;
            transform.forward = gameCamera.transform.forward;
            tower = transform.root.GetComponent<TowerBase>();
        }

        private void Update()
        {
            levelText.text = tower.Level.ToString();
        }
    }
}