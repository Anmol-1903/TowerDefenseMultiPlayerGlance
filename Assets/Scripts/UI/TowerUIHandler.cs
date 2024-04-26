using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class TowerUIHandler : MonoBehaviour
    {
        private Camera gameCamera;

        private void Start()
        {
            gameCamera = Camera.main;
            transform.GetComponentInChildren<Canvas>().worldCamera = gameCamera;
            transform.forward = gameCamera.transform.forward;
        }
    }
}