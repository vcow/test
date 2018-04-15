using Models;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public abstract class SceneControllerBase : MonoBehaviour
    {
        [SerializeField] private GameSettings _gameSettings;
        
        private const string StartSceneName = "Start";
        
        public static bool IsInitialized { get; protected set; }

        private void Awake()
        {
            GameModel.Instance.GameSettings = _gameSettings;
        }

        protected virtual void Start()
        {
            if (IsInitialized) return;

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(StartSceneName, LoadSceneMode.Single);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != StartSceneName) return;
            
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.SetActiveScene(scene);
        }

        protected abstract void InitScene();
    }
}