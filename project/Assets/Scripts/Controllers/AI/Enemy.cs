using Models;
using Properties;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.AI
{
    public class Enemy
    {
        public CardType Move()
        {
            var model = GameModel.Instance;
            if (!model.Cheeting)
            {
                return (CardType) ((int) Mathf.Round(Random.value * 2));
            }
            
            return CardType.Paper;
        }
    }
}