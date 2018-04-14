using Models;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Properties
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private Toggle _cheetingToggle;
        [SerializeField] private Slider _luckPercentSlider;
        [SerializeField] private Text _luckLabel;

        private void Start()
        {
            Assert.IsNotNull(_cheetingToggle);
            Assert.IsNotNull(_luckPercentSlider);
            Assert.IsNotNull(_luckLabel);
            
            var gameModel = GameModel.Instance;
            _cheetingToggle.isOn = gameModel.Cheeting;
            _luckLabel.text = GetPercentLabel(gameModel.LuckPercent);
            _luckPercentSlider.minValue = 0;
            _luckPercentSlider.maxValue = 1;
            _luckPercentSlider.value = gameModel.LuckPercent;
            
            _cheetingToggle.onValueChanged.AddListener(OnCheetingToggle);
            _luckPercentSlider.onValueChanged.AddListener(OnLuckSlider);
        }

        private void OnDestroy()
        {
            _cheetingToggle.onValueChanged.RemoveListener(OnCheetingToggle);
            _luckPercentSlider.onValueChanged.RemoveListener(OnLuckSlider);
        }

        private static string GetPercentLabel(float percent)
        {
            return string.Format("Luck: {0}%", Mathf.Round(Mathf.Clamp01(percent) * 100f));
        }

        private static void OnCheetingToggle(bool value)
        {
            GameModel.Instance.Cheeting = value;
        }

        private void OnLuckSlider(float value)
        {
            GameModel.Instance.LuckPercent = value;
            _luckLabel.text = GetPercentLabel(value);
        }
    }
}