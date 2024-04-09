using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnitySingleton;

namespace Core
{
    public class GameManager : PersistentMonoSingleton<GameManager>
    {
        public UnityAction OnGameStart { get; set; }
        public UnityAction<bool> OnGameEnd { get; set; }

        [field: SerializeField] public SceneContainerScriptable SceneContainer { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            StartCoroutine(LoadSceneContainer());

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
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

            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneContainer.MainMenuScene);
        }
    }
}