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

        public Tween MoveUser(bool initialize, float delay = 0, bool animateScores = false)
        {
            if (initialize)
            {
                _gameScene.LeftButton.anchoredPosition = new Vector2(-100, 40);
                _gameScene.RightButton.anchoredPosition = new Vector2(100, 40);
                _gameScene.GoButton.anchoredPosition = new Vector2(0, -120);
                _gameScene.OkButton.anchoredPosition = new Vector2(0, -120);

                if (animateScores)
                {
                    _gameScene.Scores.anchoredPosition = new Vector2(0, 65);
                }
            }

            if (animateScores)
            {
                _gameScene.Scores.DOAnchorPosY(-70f, 1f).SetDelay(2.5f);
            }

            return DOTween.Sequence().Append(_gameScene.UserCarousell.transform.DOMoveY(0.4f, 2f).SetEase(Ease.OutBack))
                .Join(DOTween.To(
                    value =>
                    {
                        _gameScene.UserCarousell.CardsGroup.transform.rotation = Quaternion.Euler(0, value, 0);
                    }, 360f, 0, 3f).SetEase(Ease.OutBack)).Join(_gameScene.LeftButton.DOAnchorPosX(140f, 1f)
                    .SetEase(Ease.OutCubic).SetDelay(1.5f)).Join(_gameScene.RightButton.DOAnchorPosX(-140f, 1f)
                    .SetEase(Ease.OutCubic)).Join(_gameScene.GoButton.DOAnchorPosY(140f, 1f).SetEase(Ease.OutCubic));
        }
    }
}