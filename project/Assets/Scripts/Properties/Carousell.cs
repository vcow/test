using System.Collections.Generic;
using UnityEngine;

namespace Properties
{
    [ExecuteInEditMode]
    public class Carousell : MonoBehaviour
    {
        [SerializeField] private GameObject _lookAtPoint;
        [SerializeField] private GameObject _cardsGroup;
        [SerializeField] private List<Card> _cards;

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
    }
}