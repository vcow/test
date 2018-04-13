using UnityEngine;

namespace Settings
{
    public enum CardType
    {
        Stone,
        Scissors,
        Paper
    }
    
    public class CardSettings : MonoBehaviour
    {
        public CardType Type;
    }
}