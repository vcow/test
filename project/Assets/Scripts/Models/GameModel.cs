using Properties;
using Settings;
using UnityEngine;

namespace Models
{
    public class GameModel
    {
        private static GameModel _instance;

        public bool Cheeting;
        public float LuckPercent;

        public CardType UserCard;
        public CardType EnemyCard;

        private GameModel()
        {
            Cheeting = GameSettings.Instance.Cheeting;
            LuckPercent = Mathf.Clamp01(GameSettings.Instance.LuckPercent);
        }

        public static GameModel Instance
        {
            get { return _instance ?? (_instance = new GameModel()); }
        }

        public bool IsUserWin()
        {
            return UserCard == CardType.Paper && EnemyCard != CardType.Scissors
                   || UserCard == CardType.Scissors && EnemyCard != CardType.Stone
                   || UserCard == CardType.Stone && EnemyCard != CardType.Paper;
        }

        public bool IsEnemyWin()
        {
            return EnemyCard == CardType.Paper && UserCard != CardType.Scissors
                   || EnemyCard == CardType.Scissors && UserCard != CardType.Stone
                   || EnemyCard == CardType.Stone && UserCard != CardType.Paper;
        }
    }
}