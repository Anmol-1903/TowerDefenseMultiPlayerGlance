using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnitySingleton;
using ConnectionStatus = Core.GameEnums.ConnectionStatus;

namespace Core
{
    public class GameManager : PersistentMonoSingleton<GameManager>
    {
        public static string GameVersion { get; private set; }

        public ConnectionStatus ConStatus { get; set; }

        public UnityAction OnGameStart { get; set; }
        public UnityAction<bool> OnGameEnd { get; set; }

        [field: SerializeField] public SceneContainerScriptable SceneContainer { get; private set; }
        //todo Add GameSettings scriptable ref same as SceneContainerScriptable

        protected override void Awake()
        {
            base.Awake();

            StartCoroutine(IntializeGame());
        }

        private IEnumerator IntializeGame()
        {
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
    }
}