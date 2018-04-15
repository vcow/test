using Properties;
using UnityEngine;

namespace Controllers
{
    public interface IGameSceneController
    {
        RectTransform LeftButton { get; }
        RectTransform RightButton { get; }
        RectTransform GoButton { get; }
        RectTransform OkButton { get; }
        RectTransform Scores { get; }
        
        ICarousell UserCarousell { get; }
        ICarousell EnemyCarousell { get; }
        
        ICard UserCard { get; }
        ICard EnemyCard { get; }
    }
}