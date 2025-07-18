using System;

namespace Assets.Scripts.GamePlay.Enemis
{
    public interface IHealthable
    {
        event Action<uint, uint> Damaged;
        uint MaxHealth { get; }
    }
}