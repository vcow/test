using DG.Tweening;
using Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Controllers
{
    public class StartSceneController : SceneControllerBase
    {
        [SerializeField] private SettingsWindow _settingsWindow;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _startButton;

        private bool _settingsIsOpened;
        private Tween _settingsWindowTween;

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
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartButton);
            _settingsButton.onClick.RemoveListener(OnSettingsButton);
        }

        private void OnStartButton()
        {
        }

        private void OnSettingsButton()
        {
            _settingsIsOpened = !_settingsIsOpened;

            var animator = _settingsButton.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("IsOpened", _settingsIsOpened);
            }

            if (_settingsWindowTween != null)
            {
                _settingsWindowTween.Complete();
            }

            if (_settingsIsOpened)
            {
                _settingsWindowTween = ((RectTransform) _settingsWindow.transform).DOAnchorPosY(-183, 0.5f);
            }
            else
            {
                _settingsWindowTween = ((RectTransform) _settingsWindow.transform).DOAnchorPosY(160, 0.3f);
            }
        }
    }
}