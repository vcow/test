﻿using Properties;
using UniRx;

namespace Models
{
    public class GameModel
    {
        private static GameModel _instance;

        public readonly IntReactiveProperty Round;
        public readonly IntReactiveProperty UserScores;
        public readonly IntReactiveProperty EnemyScores;

        public readonly ReactiveProperty<CardType> UserCard;
        public readonly ReactiveProperty<CardType> EnemyCard;

        public bool Cheeting;
        public float LuckPercent = 0.5f;

        private GameModel()
        {
            Round = new IntReactiveProperty(1);
            UserScores = new IntReactiveProperty(0);
            EnemyScores = new IntReactiveProperty(0);
            
            UserCard = new ReactiveProperty<CardType>(CardType.Stone);
            EnemyCard = new ReactiveProperty<CardType>(CardType.Stone);
        }

        public static GameModel Instance
        {
            get { return _instance ?? (_instance = new GameModel()); }
        }
    }
}