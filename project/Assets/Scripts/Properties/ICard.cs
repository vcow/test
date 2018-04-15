using UnityEngine;

namespace Properties
{
    public interface ICard
    {
        Transform transform { get; }
        Color Color { get; set; }
    }
}