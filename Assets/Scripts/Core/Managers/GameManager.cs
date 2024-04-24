using UI;
using Util;
using Networking;
using UnityEngine;
using UnitySingleton;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;
using ConnectionStatus = Core.GameEnums.ConnectionStatus;

namespace Core
{
    public class GameManager : PersistentMonoSingleton<GameManager>
    {
        public ConnectionStatus ConStatus { get; set; }

        public UnityAction<int> OnGameStart { get; set; }//parm for no of bots
        public UnityAction<bool> OnGameEnd { get; set; } //parm for isWon or not

        [field: SerializeField, Disable] public SceneContainerScriptable SceneContainer { get; private set; }
        [field: SerializeField, Disable] public GameSettings GameSettings { get; private set; }

        //todo Add GameSettings scriptable ref same as SceneContainerScriptable

        #region PublicMethods

        public void PlayGame()
        {
            OnGameStart = null;
            OnGameEnd = null;
            StartCoroutine(HelperCoroutine.LoadScene(SceneContainer.TutorialLevelScenes[0]));
        }

        public void BackToMainMenu()
        {
            OnGameStart = null;
            OnGameEnd = null;
            StartCoroutine(HelperCoroutine.LoadScene(SceneContainer.MainMenuScene));
        }

        #endregion PublicMethods

        protected override void Awake()
        {
            base.Awake();

            StartCoroutine(Intialize());
        }

        private IEnumerator Intialize()
        {
            LoadingManager.CreateInstance();
            yield return StartCoroutine(LoadingManager.Instance.GetLoadingScreenObject());
            LoadingManager.Instance.ShowLoadingScreen();

            Audio.AudioManager.CreateInstance();

            yield return StartCoroutine(HelperCoroutine.LoadDataFromResources("Scriptable/SceneContainer",
                (data) => SceneContainer = data as SceneContainerScriptable));
            yield return StartCoroutine(HelperCoroutine.LoadDataFromResources("Scriptable/GameSettings",
                    (data) => GameSettings = data as GameSettings));
            yield return StartCoroutine(HelperCoroutine.LoadDataFromResources("Scriptable/AudioContainer",
                (data) => Audio.AudioManager.Instance.LoadContainer(data as Audio.AudioContainer)));

            GameSettings.LoadData();

            AddSceneEvents();

            //todo Do Photon Init here!!
            NetworkManager.CreateInstance();
            NetworkManager.Instance.InitializePhoton();
            float startTime = Time.time;
            float timeoutDuration = 10f; // Timeout duration in seconds
            bool connected = false;
            // wait for 10 second to connect
            while (Time.time - startTime < timeoutDuration && !connected)
            {
                connected = NetworkManager.Instance.IsConnected;

                yield return null;
            }
            // if connection is successfull in 10 second set ConStatus to Connected
            if (connected)
            {
                ConStatus = ConnectionStatus.Connected;
            }
            // if not setConStatus to Disconnected
            if (!connected)
            {
                ConStatus = ConnectionStatus.Disconnected;
            }
            // If not connected within timeout duration, set connection status to Disconnected
            while (ConStatus == ConnectionStatus.IDK)
            {
                yield return null;
            }

            GameSettings.SaveData();

            StartCoroutine(HelperCoroutine.LoadScene(SceneContainer.MainMenuScene, showLoadingScreen: false));
        }

        private void SceneManager_sceneUnloaded(Scene scene)
        {
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == SceneContainer.MainMenuScene)
            {
                MainMenuManager.CreateInstance();
                LoadingManager.Instance.HideLoadingScreen();
            }
            //gamepaly scene is loaded considering offline for now!
            if (scene.name != SceneContainer.MainMenuScene && scene.name != SceneContainer.SplashScene) //can be replace by better conditions
            {
                Tower.TowerTracker.CreateInstance();
                PathHandler.PathManager.CreateInstance();
                Troop.TroopPooler.CreateInstance();
                StartCoroutine(GameSceneInit());
            }
        }

        private void AddSceneEvents()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

        private IEnumerator GameSceneInit()
        {
            Tower.TowerTracker.Instance.Init();
            yield return StartCoroutine(PathHandler.PathManager.Instance.GetPathData());
            yield return StartCoroutine(Troop.TroopPooler.Instance.GetTroopPoolData());
            LoadingManager.Instance.HideLoadingScreen();
            yield return StartCoroutine(HelperCoroutine.Countdown(3,
            onTimerUpdate: (val) =>
            {
                val.Log();
                //SomeTextEffect?
            }, onComplete: () =>
            {
                "GameStart!".Log();
                OnGameStart?.Invoke(4);
            }));
        }
    }
}