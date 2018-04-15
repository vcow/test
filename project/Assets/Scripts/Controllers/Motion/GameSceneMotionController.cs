using DG.Tweening;
using UnityEngine;

namespace Controllers.Motion
{
    public class GameSceneMotionController
    {
        private readonly IGameSceneController _gameScene;

        public GameSceneMotionController(IGameSceneController gameScene)
        {
            _gameScene = gameScene;
        }

        public Tween ShowStartUi(bool initialize, float delay = 0, bool animateScores = false)
        {
            if (initialize)
            {
                _gameScene.LeftButton.anchoredPosition = new Vector2(-100, 40);
                _gameScene.RightButton.anchoredPosition = new Vector2(100, 40);
                _gameScene.GoButton.anchoredPosition = new Vector2(0, -120);

                if (animateScores)
                {
                    _gameScene.Scores.anchoredPosition = new Vector2(0, 65);
                }
            }

            var seq = DOTween.Sequence();
            seq.Append(_gameScene.LeftButton.DOAnchorPosX(140f, 1f).SetEase(Ease.OutCubic).SetDelay(delay))
                .Join(_gameScene.RightButton.DOAnchorPosX(-140f, 1f).SetEase(Ease.OutCubic))
                .Join(_gameScene.GoButton.DOAnchorPosY(140f, 1f).SetEase(Ease.OutCubic));

            if (animateScores)
            {
                seq.Join(_gameScene.Scores.DOAnchorPosY(-70f, 1f));
            }

            return seq;
        }

        public Tween HideStartUi(bool initialize, float delay = 0, bool animateScores = false)
        {
            if (initialize)
            {
                _gameScene.LeftButton.anchoredPosition = new Vector2(140, 40);
                _gameScene.RightButton.anchoredPosition = new Vector2(-140, 40);
                _gameScene.GoButton.anchoredPosition = new Vector2(0, 140);

                if (animateScores)
                {
                    _gameScene.Scores.anchoredPosition = new Vector2(0, -70);
                }
            }

            var seq = DOTween.Sequence();
            seq.Append(_gameScene.LeftButton.DOAnchorPosX(-100f, 0.7f).SetEase(Ease.InBack).SetDelay(delay))
                .Join(_gameScene.RightButton.DOAnchorPosX(100f, 0.7f).SetEase(Ease.InBack))
                .Join(_gameScene.GoButton.DOAnchorPosY(-120f, 0.7f).SetEase(Ease.InBack));

            if (animateScores)
            {
                seq.Join(_gameScene.Scores.DOAnchorPosY(65f, 1f));
            }

            return seq;
        }

        public Tween ShowRepeatUi(bool initialize, float delay = 0)
        {
            if (initialize)
            {
                _gameScene.OkButton.anchoredPosition = new Vector2(0, -120);
            }

            return _gameScene.OkButton.DOAnchorPosY(140f, 1f).SetEase(Ease.OutCubic).SetDelay(delay);
        }

        public Tween HideRepeatUi(bool initialize, float delay = 0)
        {
            if (initialize)
            {
                _gameScene.OkButton.anchoredPosition = new Vector2(0, 140);
            }

            return _gameScene.OkButton.DOAnchorPosY(-120f, 0.7f).SetEase(Ease.InBack).SetDelay(delay);
        }

        public Tween ShowUserCarousell(float delay = 0)
        {
            _gameScene.UserCarousell.transform.position = new Vector3(0, 12, 0);

            return DOTween.Sequence().Append(_gameScene.UserCarousell.transform.DOMoveY(0.4f, 2f)
                    .SetEase(Ease.OutBack).SetDelay(delay))
                .Join(DOTween.To(
                    value =>
                    {
                        _gameScene.UserCarousell.CardsGroup.transform.localRotation = Quaternion.Euler(0, value, 0);
                    }, 360f, 0, 3f).SetEase(Ease.OutBack));
        }

        public Tween HideUserCarousell(float delay = 0)
        {
            _gameScene.UserCarousell.transform.position = new Vector3(0, 0.4f, 0);

            return DOTween.Sequence().Append(_gameScene.UserCarousell.transform.DOMoveY(12, 1f).SetEase(Ease.InBack)
                    .SetDelay(delay)).Join(_gameScene.UserCard.transform.DOLookAt(Camera.main.transform.position, 1f)
                    .SetEase(Ease.InCubic)).Join(_gameScene.UserCard.transform.DOMove(new Vector3(-2, 0.4f, -3), 1f)
                    .SetEase(Ease.OutCubic))
                .Append(_gameScene.UserCard.transform.DOLookAt(Camera.main.transform.position, 0.5f)
                    .SetEase(Ease.OutBack));
        }

        public Tween ShowEnemyCarousell(float delay = 0)
        {
            _gameScene.EnemyCarousell.transform.position = new Vector3(4, 12, 4);
            _gameScene.EnemyCarousell.transform.rotation = Quaternion.Euler(0, 180, 0);

            return DOTween.Sequence().Append(_gameScene.EnemyCarousell.transform.DOMoveY(0, 1.5f)
                    .SetEase(Ease.OutBack).SetDelay(delay))
                .Join(DOTween.To(
                    value =>
                    {
                        _gameScene.EnemyCarousell.CardsGroup.transform.localRotation = Quaternion.Euler(0, value, 0);
                    }, -240f, 0, 2f).SetEase(Ease.OutBack));
        }

        public Tween HideEnemyCarousell(float delay = 0)
        {
            _gameScene.EnemyCarousell.transform.position = new Vector3(4, 0, 4);

            return DOTween.Sequence().Append(_gameScene.EnemyCarousell.transform.DOMoveY(12, 0.7f).SetEase(Ease.InBack)
                    .SetDelay(delay)).Join(_gameScene.EnemyCard.transform.DOLookAt(Camera.main.transform.position, 1f)
                    .SetEase(Ease.InCubic).SetDelay(0.5f))
                .Join(_gameScene.EnemyCard.transform.DOMove(new Vector3(2, 0.4f, -3), 1f).SetEase(Ease.OutCubic))
                .Append(_gameScene.EnemyCard.transform.DOLookAt(Camera.main.transform.position, 0.5f)
                    .SetEase(Ease.OutBack));
        }

        public Tween UserWin(float delay = 0)
        {
            return _gameScene.UserCard.transform.DOMoveY(8.5f, 1.5f).SetEase(Ease.InCubic).SetDelay(delay);
        }

        public Tween UserLose(float delay = 0)
        {
            var r = _gameScene.UserCard.transform.eulerAngles;
            r.x -= 90;
            return DOTween.Sequence().Append(DOTween.To(
                    value => { _gameScene.UserCard.Color = new Color(1, 1, 1, value); }, 1, 0, 3f))
                .SetEase(Ease.OutCubic).SetDelay(delay).Join(_gameScene.UserCard.transform.DOLocalRotate(r, 2f)
                    .SetEase(Ease.OutBounce).SetDelay(1f));
        }

        public Tween EnemyWin(float delay = 0)
        {
            return _gameScene.EnemyCard.transform.DOMoveY(8.5f, 1.5f).SetEase(Ease.InCubic).SetDelay(delay);
        }

        public Tween EnemyLose(float delay = 0)
        {
            var r = _gameScene.EnemyCard.transform.eulerAngles;
            r.x -= 90;
            return DOTween.Sequence().Append(DOTween.To(
                    value => { _gameScene.EnemyCard.Color = new Color(1, 1, 1, value); }, 1, 0, 3f))
                .SetEase(Ease.OutCubic).SetDelay(delay).Join(_gameScene.EnemyCard.transform.DOLocalRotate(r, 2f)
                    .SetEase(Ease.OutBounce).SetDelay(1f));
        }
    }
}