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
            _userCarousell.CardsGroup.transform.rotation = new Quaternion(0, -300, 0, 0);
            
            ((RectTransform) _leftButtonContainer.transform).anchoredPosition = new Vector2(-100, 40);
            ((RectTransform) _rightButtonContainer.transform).anchoredPosition = new Vector2(100, 40);
            ((RectTransform) _goButtonContainer.transform).anchoredPosition = new Vector2(0, -120);
            ((RectTransform) _okButtonContainer.transform).anchoredPosition = new Vector2(0, -120);

            _isAnimated = true;
            DOTween.Sequence().Append(carousellInstance.transform.DOMoveY(0.4f, 2f).SetEase(Ease.OutBack))
                .Join(_userCarousell.CardsGroup.transform.DORotate(new Vector3(0, 0, 0), 3f).SetEase(Ease.OutBack))
                .Join(((RectTransform) _leftButtonContainer.transform).DOAnchorPosX(140f, 1f)
                    .SetEase(Ease.OutCubic).SetDelay(1.5f))
                .Join(((RectTransform) _rightButtonContainer.transform).DOAnchorPosX(-140f, 1f).SetEase(Ease.OutCubic))
                .Join(((RectTransform) _goButtonContainer.transform).DOAnchorPosY(140f, 1f).SetEase(Ease.OutCubic));
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