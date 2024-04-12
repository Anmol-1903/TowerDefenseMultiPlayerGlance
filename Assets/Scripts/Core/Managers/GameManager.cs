using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnitySingleton;
using Util;
using ConnectionStatus = Core.GameEnums.ConnectionStatus;

namespace Core
{
    public class GameManager : PersistentMonoSingleton<GameManager>
    {
        public static string GameVersion { get; private set; }

        public ConnectionStatus ConStatus { get; set; }

        public UnityAction OnGameStart { get; set; }
        public UnityAction<bool> OnGameEnd { get; set; }

        [field: SerializeField, Disable] public SceneContainerScriptable SceneContainer { get; private set; }
        //todo Add GameSettings scriptable ref same as SceneContainerScriptable

        #region PublicMethods

        public void PlayGame()
        {
            StartCoroutine(LoadTutorialLevel());
        }

        public void BackToMainMenu()
        {
            StartCoroutine(LoadMainMenu());
        }

        #endregion PublicMethods

        protected override void Awake()
        {
            base.Awake();

            StartCoroutine(IntializeGame());
        }

        private IEnumerator IntializeGame()
        {
            LoadingManager.CreateInstance();
            yield return StartCoroutine(LoadingManager.Instance.GetLoadingScreenObject());
            LoadingManager.Instance.ShowLoadingScreen();

            GameVersion = Application.version;
            yield return StartCoroutine(LoadSceneContainer());

            //start corountine for loading GameSettings
            //yield return StartCorountine(LoadGameSettings());
            AddSceneEvents();

            //todo Do Photon Init here!!
            // wait for 10 second to connect
            // if connection is successfull in 10 second set ConStatus to Connected
            // if not setConStatus to Disconnected

            while (ConStatus == ConnectionStatus.IDK)
            {
                yield return null;
            }

            AudioManager.CreateInstance();
            StartCoroutine(LoadMainMenu());
        }

        private void SceneManager_sceneUnloaded(Scene scene)
        {
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == SceneContainer.MainMenuScene)
            {
                UI.MainMenuManager.CreateInstance();
            }
            if (scene.name != SceneContainer.MainMenuScene && scene.name != SceneContainer.SplashScene) //can be replace by better conditions
            {
                Tower.TowerTracker.CreateInstance();
                Tower.TowerTracker.Instance.Init();
                PathHandler.PathManager.CreateInstance();
                StartCoroutine(GameStartCountDown());
            }
        }

        private IEnumerator LoadSceneContainer()
        {
            ResourceRequest request = Resources.LoadAsync("SceneContainer");
            while (!request.isDone)
            {
                yield return null;
            }

            if (request.asset != null)
            {
                SceneContainer = request.asset as SceneContainerScriptable;
            }
        }

        //todo Load the game settings scriptable from Resources
        // same as LoadSceneContainer()
        /*
         private IEnumerator LoadGameSettings()
         {
            //After Getting GameSettings
            //Call public method that load/reload gamesetttings data
         }*/

        private void AddSceneEvents()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

        private IEnumerator LoadMainMenu()
        {
            LoadingManager.Instance.ShowLoadingScreen();
            AsyncOperation operation = SceneManager.LoadSceneAsync(SceneContainer.MainMenuScene);
            while (!operation.isDone)
            {
                yield return null;
            }
            LoadingManager.Instance.HideLoadingScreen();
        }

        private IEnumerator LoadTutorialLevel()
        {
            LoadingManager.Instance.ShowLoadingScreen();
            AsyncOperation operation = SceneManager.LoadSceneAsync(SceneContainer.TutorialLevelScenes[0]);
            while (!operation.isDone)
            {
                yield return null;
            }
            LoadingManager.Instance.HideLoadingScreen();
        }

        private IEnumerator GameStartCountDown()
        {
            float timer = 0;
            while (timer <= 3)
            {
                timer += Time.deltaTime;
                timer.Log(this);
                yield return null;
            }
            $"GAME START!!  {timer:0}".Log();
        }
    }
}