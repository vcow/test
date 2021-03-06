﻿using System;
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

        /// <summary>
        /// Группа карт в карусели.
        /// </summary>
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

        /// <summary>
        /// Заполнить карты в карусели уникальными значкниями.
        /// </summary>
        public void FillCards()
        {
            for (int i = 0, rawType = (int) Math.Round(Random.value * 100); i < _cards.Count; ++i)
            {
                var type = (CardType) (rawType++ % 3);
                var card = _cards[i];
                if (card != null)
                {
                    card.CardType = type;
                }
            }
        }

        /// <summary>
        /// Крутить карусель вправо.
        /// </summary>
        /// <returns>Анимация перехода.</returns>
        public Tween MoveRight()
        {
            DOTween.Clear(_cardsGroup);
            var step = 360f / _cards.Count;
            var ang = Mathf.Round(_cardsGroup.transform.rotation.eulerAngles.y / step) * step;
            return _cardsGroup.transform.DORotate(new Vector3(0, ang + step), 0.5f).SetEase(Ease.OutBack);
        }

        /// <summary>
        /// Крутить карусель влево.
        /// </summary>
        /// <returns>Анимация перехода.</returns>
        public Tween MoveLeft()
        {
            DOTween.Clear(_cardsGroup);
            var step = 360f / _cards.Count;
            var ang = Mathf.Round(_cardsGroup.transform.rotation.eulerAngles.y / step) * step;
            return _cardsGroup.transform.DORotate(new Vector3(0, ang - step), 0.5f).SetEase(Ease.OutBack);
        }

        /// <summary>
        /// Выбранная карта. Определяется как карта с наименьшим углом отклонения от точки фокуса.
        /// </summary>
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

        /// <summary>
        /// Извлеч карту из карусели.
        /// </summary>
        /// <param name="extractedCard">Извлекаемая карта.</param>
        /// <returns>Извлеченная карта.</returns>
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