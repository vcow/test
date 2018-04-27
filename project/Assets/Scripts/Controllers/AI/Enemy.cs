using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Properties;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.AI
{
    public class Enemy
    {
        /// <summary>
        /// Сделать ход.
        /// </summary>
        /// <returns>Ход соперника.</returns>
        public CardType Move()
        {
            const float prob = 1f / 3f;
            float stoneProb, scissorsProb, paperProb;
            var model = GameModel.Instance;
            if (!model.Cheeting)
            {
                stoneProb = prob;
                scissorsProb = prob;
                paperProb = prob;
            }
            else
            {
                var userWinProb = prob * 2f * Mathf.Clamp01(model.LuckPercent);
                var deadHeatProb = Mathf.Clamp01(1.0f - userWinProb) * prob; // Вероятность выпадения ничьей.
                var userLoseProb = Mathf.Clamp01(1.0f - (userWinProb + deadHeatProb));

                switch (model.UserCard)
                {
                    case CardType.Paper:
                        stoneProb = userWinProb;
                        scissorsProb = userLoseProb;
                        paperProb = deadHeatProb;
                        break;
                    case CardType.Scissors:
                        stoneProb = userLoseProb;
                        scissorsProb = deadHeatProb;
                        paperProb = userWinProb;
                        break;
                    case CardType.Stone:
                        stoneProb = deadHeatProb;
                        scissorsProb = userWinProb;
                        paperProb = userLoseProb;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            var probs = new Dictionary<CardType, float>
            {
                {CardType.Stone, Random.value * stoneProb},
                {CardType.Scissors, Random.value * scissorsProb},
                {CardType.Paper, Random.value * paperProb}
            };
            return probs.Aggregate((p1, p2) => p1.Value > p2.Value ? p1 : p2).Key;
        }
    }
}