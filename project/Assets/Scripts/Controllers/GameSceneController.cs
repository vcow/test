using Controllers.AI;
using Controllers.Motion;
using DG.Tweening;
using Models;
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
        private bool _uiIsLocked;

        private Enemy _enemy;

        /// <summary>
        /// Кнопка листания влево.
        /// </summary>
        RectTransform IGameSceneController.LeftButton
        {
            get { return (RectTransform) _leftButton.transform.parent.transform; }
        }

        /// <summary>
        /// Кнопка листания вправо.
        /// </summary>
        RectTransform IGameSceneController.RightButton
        {
            get { return (RectTransform) _rightButton.transform.parent.transform; }
        }

        /// <summary>
        /// Кнопка выбора карты.
        /// </summary>
        RectTransform IGameSceneController.GoButton
        {
            get { return (RectTransform) _goButton.transform.parent.transform; }
        }

        /// <summary>
        /// Кнопка перехода на следующий раунд.
        /// </summary>
        RectTransform IGameSceneController.OkButton
        {
            get { return (RectTransform) _okButton.transform.parent.transform; }
        }

        /// <summary>
        /// Поле отображения счета.
        /// </summary>
        RectTransform IGameSceneController.Scores
        {
            get { return (RectTransform) _scoresLabel.transform; }
        }

        /// <summary>
        /// Текущая карусель игрока.
        /// </summary>
        ICarousell IGameSceneController.UserCarousell
        {
            get { return _userCarousell; }
        }

        /// <summary>
        /// Текущая карусель противника.
        /// </summary>
        ICarousell IGameSceneController.EnemyCarousell
        {
            get { return _enemyCarousell; }
        }

        /// <summary>
        /// Текущая карта игрока.
        /// </summary>
        ICard IGameSceneController.UserCard
        {
            get { return _userCard; }
        }

        /// <summary>
        /// Текущая карта противника.
        /// </summary>
        ICard IGameSceneController.EnemyCard
        {
            get { return _enemyCard; }
        }

        protected override void Start()
        {
            base.Start();
            if (!IsInitialized) return;

            InitScene();
        }

        private bool UiIsLocked
        {
            get { return _uiIsLocked; }
            set
            {
                // ReSharper disable once RedundantCheckBeforeAssignment
                if (value == _uiIsLocked) return;
                _uiIsLocked = value;
#if !UNITY_EDITOR
                foreach (var btn in new[] {_leftButton, _rightButton, _goButton, _okButton})
                {
                    btn.enabled = !_uiIsLocked;
                }
    #endif
            }
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

            _enemy = new Enemy();
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
            if (UiIsLocked || _userCarousell == null) return;

            UiIsLocked = true;
            _userCarousell.MoveLeft().onComplete += () => UiIsLocked = false;
        }

        private void OnRightButton()
        {
            if (UiIsLocked || _userCarousell == null) return;

            UiIsLocked = true;
            _userCarousell.MoveRight().onComplete += () => UiIsLocked = false;
        }

        private void OnGoButton()
        {
            if (UiIsLocked || _userCarousell == null) return;

            _userCard = _userCarousell.ExtractCard(_userCarousell.SelectedCard);
            GameModel.Instance.UserCard = _userCard.CardType;

            DoEnemyMove();
        }

        private void OnOkButton()
        {
            if (UiIsLocked) return;

            UiIsLocked = true;
            _motionController.HideRepeatUi(true);

            DoUserMove();
        }

        /// <summary>
        /// Старт раунда.
        /// </summary>
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

            UiIsLocked = true;
            _motionController.ShowStartUi(initialize, delay + 1.5f, animateScores).onComplete +=
                () => UiIsLocked = false;
            _motionController.ShowUserCarousell(delay);
        }

        /// <summary>
        /// Ход противника.
        /// </summary>
        private void DoEnemyMove()
        {
            Assert.IsNotNull(_carousellPrefab);
            Assert.IsNotNull(_userCarousell);
            Assert.IsNotNull(_userCard);

            Assert.IsNull(_enemyCarousell);
            Assert.IsNull(_enemyCard);

            var carousellInstance = Instantiate(_carousellPrefab);
            _enemyCarousell = carousellInstance.GetComponent<Carousell>();
            Assert.IsNotNull(_enemyCarousell);

            UiIsLocked = true;
            _motionController.HideStartUi(true, 0).onComplete += () => UiIsLocked = false;

            _motionController.HideUserCarousell().onComplete += () =>
            {
                Destroy(_userCarousell.gameObject);
                _userCarousell = null;
            };

            _motionController.ShowEnemyCarousell(1.7f).onComplete += () =>
            {
                _enemyCard = _enemyCarousell.ExtractCard(_enemyCarousell.SelectedCard);
                Assert.IsNotNull(_enemyCard);

                var enemyCardType = _enemy.Move();
                _enemyCard.CardType = enemyCardType;
                GameModel.Instance.EnemyCard = enemyCardType;

                _motionController.HideEnemyCarousell(0.7f).onComplete += () =>
                {
                    Destroy(_enemyCarousell.gameObject);
                    _enemyCarousell = null;

                    DoResult();
                };
            };
        }

        /// <summary>
        /// Показ результата.
        /// </summary>
        private void DoResult()
        {
            Assert.IsNotNull(_userCard);
            Assert.IsNotNull(_enemyCard);

            var model = GameModel.Instance;
            Tween userCardAnimation, enemyCardAnimation;
            if (model.IsUserWin(false))
            {
                userCardAnimation = _motionController.UserWin();
                model.UserScores += 1;
            }
            else
            {
                userCardAnimation = _motionController.UserLose();
            }

            if (model.IsEnemyWin(false))
            {
                enemyCardAnimation = _motionController.EnemyWin();
                model.EnemyScores += 1;
            }
            else
            {
                enemyCardAnimation = _motionController.EnemyLose();
            }

            DOTween.Sequence().Append(userCardAnimation).Join(enemyCardAnimation).onComplete += () =>
            {
                Destroy(_userCard.gameObject);
                _userCard = null;

                Destroy(_enemyCard.gameObject);
                _enemyCard = null;

                UiIsLocked = true;
                _motionController.ShowRepeatUi(true).onComplete += () => UiIsLocked = false;
            };

            UpdateScores();
        }

        private void UpdateScores()
        {
            Assert.IsNotNull(_scoresLabel);

            var model = GameModel.Instance;
            _scoresLabel.text = string.Format("{0:D2}:{1:D2}", model.UserScores, model.EnemyScores);
        }
    }
}