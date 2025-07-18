using System;

namespace Assets.Scripts.GamePlay.Player.Weapons
{
    public interface IShootable
    {
        event Action BulletsCountChanged;

        uint BulletsCount { get; }
    }
}
