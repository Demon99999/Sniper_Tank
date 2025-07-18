using System;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Destructions
{
    public interface IDestructablePart
    {
        event Action<Vector3, uint> Destructed;
        Transform Transform { get; }
    }
}