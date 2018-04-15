using System;
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

            const float deadHeatProb = 1.0f / 3.0f * 0.5f; // Вероятность выпадения ничьей.
            var userWinProb = (1.0f - deadHeatProb) * Mathf.Clamp01(model.LuckPercent);
            var userLoseProb = Mathf.Clamp01(1.0f - (deadHeatProb + userWinProb));

            var rnd = Random.value;
            if (rnd <= deadHeatProb)
            {
                // Ничья
                return model.UserCard;
            }
            else if (rnd <= deadHeatProb + userLoseProb)
            {
                // Юзер проигрывает
                switch (model.UserCard)
                {
                    case CardType.Paper: return CardType.Scissors;
                    case CardType.Scissors: return CardType.Stone;
                    case CardType.Stone: return CardType.Paper;
                }
            }
            else
            {
                // Юзер побеждает
                switch (model.UserCard)
                {
                    case CardType.Paper: return CardType.Stone;
                    case CardType.Scissors: return CardType.Paper;
                    case CardType.Stone: return CardType.Scissors;
                }
            }

            throw new NotSupportedException();
        }
    }
}