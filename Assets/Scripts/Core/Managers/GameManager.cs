using System.Collections;
using UI;
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
            StartCoroutine(HelperCoroutine.LoadScene(SceneContainer.TutorialLevelScenes[0]));
        }

        public void BackToMainMenu()
        {
            StartCoroutine(HelperCoroutine.LoadScene(SceneContainer.MainMenuScene));
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
            yield return StartCoroutine(HelperCoroutine.LoadDataFromResources("SceneContainer", (data) => SceneContainer = data as SceneContainerScriptable));

            //start corountine for loading GameSettings same as above!!

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
                MainMenuManager.CreateInstance();
            }
            if (scene.name != SceneContainer.MainMenuScene && scene.name != SceneContainer.SplashScene) //can be replace by better conditions
            {
                Tower.TowerTracker.CreateInstance();
                Tower.TowerTracker.Instance.Init();
                PathHandler.PathManager.CreateInstance();
                StartCoroutine(GameStartCountDown());
            }
        }

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