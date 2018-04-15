using UnityEngine;

namespace Properties
{
    public interface ICarousell
    {
        Transform transform { get; }
        Transform CardsGroup { get; }
    }
}