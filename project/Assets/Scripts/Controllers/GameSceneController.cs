using System;
using DG.Tweening;
using Properties;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Controllers
{
    public class GameSceneController : SceneControllerBase
    {
        [SerializeField] private GameObject _carousellPrefab;

        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _goButton;
        [SerializeField] private Button _okButton;
        
        [SerializeField] private Text _scoresLabel;

        private GameObject _leftButtonContainer;
        private GameObject _rightButtonContainer;
        private GameObject _goButtonContainer;
        private GameObject _okButtonContainer;

        private Carousell _userCarousell;
        private Carousell _enemyCarousell;

        private bool _isAnimated;

        protected override void Start()
        {
            base.Start();
            if (!IsInitialized) return;

            InitScene();
        }

        protected override void InitScene()
        {
            Assert.IsNotNull(_leftButton);
            Assert.IsNotNull(_rightButton);
            Assert.IsNotNull(_goButton);
            Assert.IsNotNull(_okButton);
            Assert.IsNotNull(_scoresLabel);

            _leftButtonContainer = _leftButton.transform.parent.gameObject;
            _rightButtonContainer = _rightButton.transform.parent.gameObject;
            _goButtonContainer = _goButton.transform.parent.gameObject;
            _okButtonContainer = _okButton.transform.parent.gameObject;
            
            IDisposable task = null;
            task = Observable.Interval(TimeSpan.FromSeconds(1)).
                ObserveOnMainThread(MainThreadDispatchType.Update).Subscribe(unit =>
                {
                    DoUserMove();
                    task.Dispose();
                });

            ((RectTransform) _scoresLabel.transform).DOAnchorPosY(-70f, 1f).SetDelay(2.5f);
            
            _leftButton.onClick.AddListener(OnLeftButton);
            _rightButton.onClick.AddListener(OnRightButton);
            _goButton.onClick.AddListener(OnGoButton);
            _okButton.onClick.AddListener(OnOkButton);
        }

        private void OnDestroy()
        {
            _leftButton.onClick.RemoveListener(OnLeftButton);
            _rightButton.onClick.RemoveListener(OnRightButton);
            _goButton.onClick.RemoveListener(OnGoButton);
            _okButton.onClick.RemoveListener(OnOkButton);
        }

        private void OnLeftButton()
        {
            if (_isAnimated || _userCarousell == null) return;

            _isAnimated = true;
            _userCarousell.MoveLeft().onComplete += () => _isAnimated = false;
        }

        private void OnRightButton()
        {
            if (_isAnimated || _userCarousell == null) return;

            _isAnimated = true;
            _userCarousell.MoveRight().onComplete += () => _isAnimated = false;
        }

        private void OnGoButton()
        {
            if (_isAnimated) return;
            
        }

        private void OnOkButton()
        {
            if (_isAnimated) return;
            
        }

        private void DoUserMove()
        {
            Assert.IsNotNull(_carousellPrefab);
            
            KillUserCarousell();
            KillEnemyCarousell();

            var carousellInstance = Instantiate(_carousellPrefab);
            _userCarousell = carousellInstance.GetComponent<Carousell>();
            Assert.IsNotNull(_userCarousell);
            
            _userCarousell.FillCards();
            
            carousellInstance.transform.position = new Vector3(0, 12, 0);
            
            ((RectTransform) _leftButtonContainer.transform).anchoredPosition = new Vector2(-100, 40);
            ((RectTransform) _rightButtonContainer.transform).anchoredPosition = new Vector2(100, 40);
            ((RectTransform) _goButtonContainer.transform).anchoredPosition = new Vector2(0, -120);
            ((RectTransform) _okButtonContainer.transform).anchoredPosition = new Vector2(0, -120);

            _isAnimated = true;
            DOTween.Sequence().Append(carousellInstance.transform.DOMoveY(0.4f, 2f).SetEase(Ease.OutBack))
                .Join(DOTween.To(value => _userCarousell.CardsGroup.transform.rotation = Quaternion.Euler(0, value, 0),
                    360f, 0, 3f).SetEase(Ease.OutBack))
                .Join(((RectTransform) _leftButtonContainer.transform).DOAnchorPosX(140f, 1f)
                    .SetEase(Ease.OutCubic).SetDelay(1.5f))
                .Join(((RectTransform) _rightButtonContainer.transform).DOAnchorPosX(-140f, 1f).SetEase(Ease.OutCubic))
                .Join(((RectTransform) _goButtonContainer.transform).DOAnchorPosY(140f, 1f).SetEase(Ease.OutCubic))
                .onComplete += () => _isAnimated = false;
        }

        private void KillUserCarousell()
        {
            if (_userCarousell == null) return;
            Destroy(_userCarousell);
            _userCarousell = null;
        }

        private void KillEnemyCarousell()
        {
            if (_enemyCarousell == null) return;
            Destroy(_enemyCarousell);
            _enemyCarousell = null;
        }
    }
}