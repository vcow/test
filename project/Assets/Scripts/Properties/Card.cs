using System;
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
        public CardType Type;

        [SerializeField] private SpriteRenderer _face;
        [SerializeField] private SpriteRenderer _shirt;
        [SerializeField] private GameObject _view;

        void ICard.UpdateCard()
        {
            Assert.IsNotNull(_face);
            Assert.IsNotNull(_shirt);

            var settings = GameSettings.Instance;
            _shirt.sprite = settings.Shirt;
            switch (Type)
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
            if (Application.isPlaying)
            {
                ((ICard) this).UpdateCard();
            }
        }
    }
}