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
            GameObject loadingScreenPrefab = null;

            yield return StartCoroutine(HelperCoroutine.LoadDataFromResources("Prefabs/LoadingCanvas", (data) => loadingScreenPrefab = data as GameObject));

            if (loadingScreenPrefab == null)
            {
                "Loading Screen is null, it is not present at \"Prefabs/LoadingCanvas\"".Log(Color.red, this);
                yield break;
            }

            loadingScreen = Instantiate(loadingScreenPrefab);
            DontDestroyOnLoad(loadingScreen);
            loadingScreen.SetActive(false);
            yield return null;
        }

        public void ShowLoadingScreen()
        {
            if (!loadingScreen.activeInHierarchy)
            {
                loadingScreen.SetActive(true);
                loadingScreen.GetComponent<CanvasGroup>().interactable = true;
                loadingScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }

        public void HideLoadingScreen()
        {
            if (loadingScreen.activeInHierarchy)
            {
                loadingScreen.SetActive(false);
                loadingScreen.GetComponent<CanvasGroup>().interactable = false;
                loadingScreen.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }
}