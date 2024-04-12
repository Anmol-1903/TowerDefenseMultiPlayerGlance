using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySingleton;
using Util;

namespace Core
{
    public class LoadingManager : PersistentMonoSingleton<LoadingManager>
    {
        [SerializeField, Disable] private GameObject loadingScreen;

        public IEnumerator GetLoadingScreenObject()
        {
            ResourceRequest request = Resources.LoadAsync("Prefabs/LoadingCanvas");
            while (!request.isDone)
            {
                yield return null;
            }

            if (request.asset == null)
            {
                "LoadingScreen is null".Log(Color.red, this);
                yield break;
            }
            loadingScreen = Instantiate(request.asset) as GameObject;
            DontDestroyOnLoad(loadingScreen);
            loadingScreen.SetActive(false);
            yield return null;
        }

        public void ShowLoadingScreen()
        {
            if (!loadingScreen.activeInHierarchy)
            {
                loadingScreen.SetActive(true);
            }
        }

        public void HideLoadingScreen()
        {
            if (loadingScreen.activeInHierarchy)
            {
                loadingScreen.SetActive(false);
            }
        }
    }
}