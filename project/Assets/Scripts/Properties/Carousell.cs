using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Properties
{
    [ExecuteInEditMode]
    public class Carousell : MonoBehaviour
    {
        [SerializeField] private GameObject _lookAtPoint;
        [SerializeField] private GameObject _cardsGroup;
        [SerializeField] private List<Card> _cards;
        
        public GameObject CardsGroup
        {
            get { return _cardsGroup; }
        }

        private void Update()
        {
            if (_lookAtPoint == null) return;
            
            var t = _lookAtPoint.transform;
            _cards.ForEach(c => c.transform.LookAt(t));
        }
        
        private void OnDrawGizmos()
        {
            if (_lookAtPoint == null) return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_lookAtPoint.transform.position, 0.25f);
        }

        public void FillCards()
        {
            var rawType = (int) Math.Round(UnityEngine.Random.value * 2);
            for (int i = 0, l = _cards.Count; i < l; ++i)
            {
                var type = (CardType) rawType;
                rawType = ++rawType % 3;
                _cards[i].CardType = type;
            }
        }

        public Tween MoveLeft()
        {
            DOTween.Clear(CardsGroup);
            var step = 360f / _cards.Count;
            var ang = Mathf.Round(CardsGroup.transform.rotation.eulerAngles.y / step) * step;
            return CardsGroup.transform.DORotate(new Vector3(0, ang + step), 0.35f).SetEase(Ease.OutCubic);
        }

        public Tween MoveRight()
        {
            DOTween.Clear(CardsGroup);
            var step = 360f / _cards.Count;
            var ang = Mathf.Round(CardsGroup.transform.rotation.eulerAngles.y / step) * step;
            return CardsGroup.transform.DORotate(new Vector3(0, ang - step), 0.35f).SetEase(Ease.OutCubic);
        }
    }
}