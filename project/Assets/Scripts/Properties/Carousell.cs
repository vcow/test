using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Properties
{
    [ExecuteInEditMode]
    public class Carousell : MonoBehaviour, ICarousell
    {
        [SerializeField] private GameObject _lookAtPoint;
        [SerializeField] private GameObject _cardsGroup;
        [SerializeField] private List<Card> _cards;
        private Transform _cardsGroup1;

        Transform ICarousell.CardsGroup
        {
            get { return _cardsGroup.transform; }
        }

        private void Update()
        {
            if (_lookAtPoint == null) return;
            
            var t = _lookAtPoint.transform;
            _cards.ForEach(c =>
            {
                if (c != null) c.transform.LookAt(t);
            });
        }
        
        private void OnDrawGizmos()
        {
            if (_lookAtPoint == null) return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_lookAtPoint.transform.position, 0.25f);
        }

        public void FillCards()
        {
            var rawType = (int) Math.Round(Random.value * 2);
            for (int i = 0, l = _cards.Count; i < l; ++i)
            {
                var type = (CardType) rawType;
                rawType = ++rawType % 3;
                var card = _cards[i];
                if (card != null)
                {
                    card.CardType = type;
                }
            }
        }

        public Tween MoveLeft()
        {
            DOTween.Clear(_cardsGroup);
            var step = 360f / _cards.Count;
            var ang = Mathf.Round(_cardsGroup.transform.rotation.eulerAngles.y / step) * step;
            return _cardsGroup.transform.DORotate(new Vector3(0, ang + step), 0.35f).SetEase(Ease.OutCubic);
        }

        public Tween MoveRight()
        {
            DOTween.Clear(_cardsGroup);
            var step = 360f / _cards.Count;
            var ang = Mathf.Round(_cardsGroup.transform.rotation.eulerAngles.y / step) * step;
            return _cardsGroup.transform.DORotate(new Vector3(0, ang - step), 0.35f).SetEase(Ease.OutCubic);
        }

        public Card SelectedCard
        {
            get
            {
                Card card = null;
                var ang = float.MaxValue;
                var groupPos = _cardsGroup.transform.position;
                var lookAtPos = _lookAtPoint.transform.position - groupPos;
                _cards.ForEach(c =>
                {
                    if (c == null) return;
                    var a = Mathf.Abs(Vector3.Angle(lookAtPos, c.transform.position - groupPos));
                    if (!(a < ang)) return;
                    ang = a;
                    card = c;
                });
                return card;
            }
        }

        public Card ExtractCard(Card extractedCard)
        {
            if (_cards.Contains(extractedCard))
            {
                _cards.Remove(extractedCard);
            }
            
            extractedCard.transform.SetParent(null);
            return extractedCard;
        }
    }
}