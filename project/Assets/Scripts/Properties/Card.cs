using System;
using Models;
using Settings;
using UnityEngine;
using UnityEngine.Assertions;

namespace Properties
{
    public enum CardType
    {
        Stone,
        Scissors,
        Paper
    }

    public class Card : MonoBehaviour, ICard
    {
        private CardType _cardType;
        private Color _color = Color.white;

        private Renderer[] _renderers;

        public CardType CardType
        {
            get { return _cardType; }
            set
            {
                if (value == _cardType) return;
                _cardType = value;
                UpdateCard();
            }
        }

        [SerializeField] private CardType _type;
        [SerializeField] private SpriteRenderer _face;
        [SerializeField] private SpriteRenderer _shirt;
        [SerializeField] private GameObject _view;

        public Color Color
        {
            get { return _color; }
            set
            {
                if (value == _color) return;
                _color = value;
                foreach (var r in _renderers)
                {
                    r.material.color = _color;
                }
            }
        }

        private void UpdateCard()
        {
            Assert.IsNotNull(_face);
            Assert.IsNotNull(_shirt);

#if UNITY_EDITOR
            var settings = GameModel.Instance.GameSettings ?? GameSettings.GetSettingsInstance();
#else
            var settings = GameModel.Instance.GameSettings;
    #endif
            _shirt.sprite = settings.Shirt;
            switch (CardType)
            {
                case CardType.Stone:
                    _face.sprite = settings.StoneFace;
                    break;
                case CardType.Scissors:
                    _face.sprite = settings.ScissorsFace;
                    break;
                case CardType.Paper:
                    _face.sprite = settings.PaperFace;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void Start()
        {
            _type = CardType;
            _renderers = GetComponentsInChildren<Renderer>();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            CardType = _type;
        }
#endif
    }
}