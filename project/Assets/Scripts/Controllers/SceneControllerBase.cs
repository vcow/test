using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public abstract class SceneControllerBase : MonoBehaviour
    {
        private const string StartSceneName = "Start";
        
        public static bool IsInitialized { get; protected set; }

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