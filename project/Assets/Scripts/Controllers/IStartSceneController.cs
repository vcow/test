using UnityEngine;

namespace Controllers
{
    public interface IStartSceneController
    {
        RectTransform StartButton { get; }
        RectTransform SettingsButton { get; }
        RectTransform SettingsWindow { get; }
    }
}