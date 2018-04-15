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

        public int UserScores;
        public int EnemyScores;

        private GameModel()
        {
            Cheeting = GameSettings.Instance.Cheeting;
            LuckPercent = Mathf.Clamp01(GameSettings.Instance.LuckPercent);
        }

        public static GameModel Instance
        {
            get { return _instance ?? (_instance = new GameModel()); }
        }

        public bool IsUserWin(bool withDeadHeat)
        {
            if (withDeadHeat && UserCard == EnemyCard)
            {
                return true;
            }
            
            return UserCard == CardType.Paper && EnemyCard == CardType.Stone
                   || UserCard == CardType.Scissors && EnemyCard == CardType.Paper
                   || UserCard == CardType.Stone && EnemyCard == CardType.Scissors;
        }

        public bool IsEnemyWin(bool withDeadHeat)
        {
            if (withDeadHeat && UserCard == EnemyCard)
            {
                return true;
            }
            
            return EnemyCard == CardType.Paper && UserCard == CardType.Stone
                   || EnemyCard == CardType.Scissors && UserCard == CardType.Paper
                   || EnemyCard == CardType.Stone && UserCard == CardType.Scissors;
        }
    }
}