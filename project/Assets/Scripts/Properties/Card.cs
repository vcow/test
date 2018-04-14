using System;
using Settings;
using UniRx;
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

    public class Card : MonoBehaviour
    {
        private CardType _cardType;

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

        private void UpdateCard()
        {
            Assert.IsNotNull(_face);
            Assert.IsNotNull(_shirt);

            var settings = GameSettings.Instance;
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
            CardType = _type;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            IDisposable task = null;
            task = Observable.Interval(TimeSpan.Zero).Subscribe(unit =>
            {
                CardType = _type;
                task.Dispose();
            });
        }
#endif
    }
}