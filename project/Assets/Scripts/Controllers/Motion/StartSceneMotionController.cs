using DG.Tweening;
using UnityEngine;

namespace Controllers.Motion
{
    public class StartSceneMotionController
    {
        private readonly IStartSceneController _startScene;

        public StartSceneMotionController(IStartSceneController startScene)
        {
            _startScene = startScene;
        }

        /// <summary>
        /// Показать UI.
        /// </summary>
        /// <param name="initialize">Переместить все элементы в исходное положение.</param>
        /// <param name="delay">Задержка анимации.</param>
        /// <returns>Анимация перехода.</returns>
        public Tween MoveUiIn(bool initialize, float delay = 0)
        {
            if (initialize)
            {
                _startScene.StartButton.anchoredPosition = new Vector2(0, -80);
                _startScene.SettingsButton.anchoredPosition = new Vector2(-110, 60);
            }

            return DOTween.Sequence().Append(_startScene.StartButton.DOAnchorPosY(145f, 1f)
                    .SetEase(Ease.OutCubic).SetDelay(delay))
                .Join(_startScene.SettingsButton.DOAnchorPosY(-80f, 1f).SetEase(Ease.OutCubic));
        }

        /// <summary>
        /// Анимация окна настроек.
        /// </summary>
        /// <param name="doOpen">Если флаг установлен, меню открывается, иначе закрывается.</param>
        /// <returns>Анимация перехода.</returns>
        public Tween MoveSettingsWindow(bool doOpen)
        {
            DOTween.Clear(_startScene.SettingsWindow);

            return doOpen
                ? _startScene.SettingsWindow.DOAnchorPosY(-183, 0.5f)
                : _startScene.SettingsWindow.DOAnchorPosY(160, 0.3f);
        }
    }
}