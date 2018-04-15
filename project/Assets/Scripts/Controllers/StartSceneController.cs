using Controllers.Motion;
using DG.Tweening;
using Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers
{
    public class StartSceneController : SceneControllerBase, IStartSceneController
    {
        [SerializeField] private SettingsWindow _settingsWindow;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _startButton;

        private bool _settingsIsOpened;

        private StartSceneMotionController _motionController;

        RectTransform IStartSceneController.StartButton
        {
            get { return (RectTransform) _startButton.transform; }
        }

        RectTransform IStartSceneController.SettingsButton
        {
            get { return (RectTransform) _settingsButton.transform.parent.transform; }
        }

        RectTransform IStartSceneController.SettingsWindow
        {
            get { return (RectTransform) _settingsWindow.transform; }
        }

        protected override void Start()
        {
            if (IsInitialized) return;

            InitScene();
            IsInitialized = true;
        }

        protected override void InitScene()
        {
            Assert.IsNotNull(_settingsWindow);
            Assert.IsNotNull(_settingsButton);
            Assert.IsNotNull(_startButton);

            _startButton.onClick.AddListener(OnStartButton);
            _settingsButton.onClick.AddListener(OnSettingsButton);
            
            _motionController = new StartSceneMotionController(this);
            _motionController.MoveUiIn(false, 2f);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartButton);
            _settingsButton.onClick.RemoveListener(OnSettingsButton);
        }

        private void OnStartButton()
        {
            _startButton.enabled = false;
            _settingsButton.enabled = false;
            
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }

        private void OnSettingsButton()
        {
            _settingsIsOpened = !_settingsIsOpened;

            var animator = _settingsButton.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("IsOpened", _settingsIsOpened);
            }

            _motionController.MoveSettingsWindow(_settingsIsOpened);
        }
    }
}