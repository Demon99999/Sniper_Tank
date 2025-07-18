using System;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Player.Aim
{
    public interface IRotationAiming
    {
        event Action<Vector2> AimShifted;
        event Action<Vector2> HandlePressed;
    }
}