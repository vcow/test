using Properties;
using Settings;
using UnityEngine;
using UnityEngine.Assertions;

namespace Models
{
    public class GameModel
    {
        private GameSettings _gameSettings;
        
        private static GameModel _instance;

        /// <summary>
        /// Режим игры (нечестный, если флаг установлен).
        /// </summary>
        public bool Cheeting;
        
        /// <summary>
        /// Процент удачи при нечестной игре.
        /// </summary>
        public float LuckPercent;

        /// <summary>
        /// Текущая выбранная карта игрока.
        /// </summary>
        public CardType UserCard;
        
        /// <summary>
        /// Текущая выбранная карта соперника.
        /// </summary>
        public CardType EnemyCard;

        /// <summary>
        /// Очки игрока.
        /// </summary>
        public int UserScores;
        
        /// <summary>
        /// Очки соперника.
        /// </summary>
        public int EnemyScores;

        /// <summary>
        /// Предустановленные настройки игры.
        /// </summary>
        public GameSettings GameSettings
        {
            get { return _gameSettings; }
            set
            {
                Assert.IsNotNull(value);
                if (value == _gameSettings) return;
                _gameSettings = value;
                
                Cheeting = _gameSettings.Cheeting;
                LuckPercent = Mathf.Clamp01(_gameSettings.LuckPercent);
            }
        }

        public static GameModel Instance
        {
            get { return _instance ?? (_instance = new GameModel()); }
        }

        /// <summary>
        /// Проверка на победу игрока.
        /// </summary>
        /// <param name="withDeadHeat">Считать победой ничью.</param>
        /// <returns>Возвращает <code>true</code>, если игрок победил в текущем раунде.</returns>
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

        /// <summary>
        /// Проверка на победу соперника.
        /// </summary>
        /// <param name="withDeadHeat">Считать победой ничью.</param>
        /// <returns>Возвращает <code>true</code>, если соперник победил в текущем раунде.</returns>
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