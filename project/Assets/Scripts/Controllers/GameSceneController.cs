using Controllers.Motion;
using Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Controllers
{
    public class GameSceneController : SceneControllerBase, IGameSceneController
    {
        [SerializeField] private GameObject _carousellPrefab;

        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _goButton;
        [SerializeField] private Button _okButton;

        [SerializeField] private Text _scoresLabel;

        private Carousell _userCarousell;
        private Carousell _enemyCarousell;

        private Card _userCard;
        private Card _enemyCard;

        private GameSceneMotionController _motionController;
        private bool _isAnimated;

        RectTransform IGameSceneController.LeftButton
        {
            get { return (RectTransform) _leftButton.transform.parent.transform; }
        }

        RectTransform IGameSceneController.RightButton
        {
            get { return (RectTransform) _rightButton.transform.parent.transform; }
        }

        RectTransform IGameSceneController.GoButton
        {
            get { return (RectTransform) _goButton.transform.parent.transform; }
        }

        RectTransform IGameSceneController.OkButton
        {
            get { return (RectTransform) _okButton.transform.parent.transform; }
        }

        RectTransform IGameSceneController.Scores
        {
            get { return (RectTransform) _scoresLabel.transform; }
        }

        ICarousell IGameSceneController.UserCarousell
        {
            get { return _userCarousell; }
        }

        ICarousell IGameSceneController.EnemyCarousell
        {
            get { return _enemyCarousell; }
        }

        Transform IGameSceneController.UserCard
        {
            get { return _userCard.transform; }
        }

        Transform IGameSceneController.EnemyCard
        {
            get { return _enemyCard.transform; }
        }

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

            _leftButton.onClick.AddListener(OnLeftButton);
            _rightButton.onClick.AddListener(OnRightButton);
            _goButton.onClick.AddListener(OnGoButton);
            _okButton.onClick.AddListener(OnOkButton);

            _motionController = new GameSceneMotionController(this);

            DoUserMove(false, 1f, true);
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
            if (_isAnimated || _userCarousell == null) return;
            DoEnemyMove();
        }

        private void OnOkButton()
        {
            if (_isAnimated) return;
        }

        private void DoUserMove(bool initialize = true, float delay = 0, bool animateScores = false)
        {
            Assert.IsNotNull(_carousellPrefab);

            Assert.IsNull(_userCarousell);
            Assert.IsNull(_enemyCarousell);
            Assert.IsNull(_userCard);
            Assert.IsNull(_enemyCard);

            var carousellInstance = Instantiate(_carousellPrefab);
            _userCarousell = carousellInstance.GetComponent<Carousell>();
            Assert.IsNotNull(_userCarousell);

            _userCarousell.FillCards();

            _isAnimated = true;
            _motionController.ShowUi(initialize, delay + 1.5f, animateScores).onComplete += () => _isAnimated = false;
            _motionController.ShowUserCarousell(delay);
        }

        private void DoEnemyMove()
        {
            Assert.IsNotNull(_carousellPrefab);
            Assert.IsNotNull(_userCarousell);

            Assert.IsNull(_enemyCarousell);
            Assert.IsNull(_userCard);
            Assert.IsNull(_enemyCard);

            _userCard = _userCarousell.ExtractCard(_userCarousell.SelectedCard);
            Assert.IsNotNull(_userCard);

            var carousellInstance = Instantiate(_carousellPrefab);
            _enemyCarousell = carousellInstance.GetComponent<Carousell>();
            Assert.IsNotNull(_enemyCarousell);

            _isAnimated = true;
            _motionController.HideUi(true, 0, true).onComplete += () => _isAnimated = false;

            _motionController.HideUserCarousell().onComplete += () =>
            {
                Destroy(_userCarousell);
                _userCarousell = null;
            };

            _motionController.ShowEnemyCarousell(1.7f).onComplete += () =>
            {
                _enemyCard = _enemyCarousell.ExtractCard(_enemyCarousell.SelectedCard);
                Assert.IsNotNull(_enemyCard);

                _motionController.HideEnemyCarousell(0.7f);
            };
        }
    }
}