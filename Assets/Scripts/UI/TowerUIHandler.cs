using Photon.Pun;
using TMPro;
using Tower;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TowerUIHandler : MonoBehaviour
    {
        private Camera gameCamera;
        private TowerBase tower;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Image[] dots;

      
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

            //do this only when its clients own tower
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].gameObject.SetActive(tower.MaxPaths > i);
                dots[i].color = tower.UsedPaths > i ? Color.white : new Color(1f, 1f, 1f, 0.4f);
            }

            
        }

        public void UpdateUIPosition(Transform pos)
        {
            transform.position = pos.position;
        }

       
    }
}