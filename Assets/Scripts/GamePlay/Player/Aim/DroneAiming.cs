using System;
using Assets.Scripts.Services.InputService;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Player.Aim
{
    public class DroneAiming : IDisposable, IRotationAiming, IShootedAiming, IAiming
    {
        private readonly IInputService _inputService;

        protected DroneAiming(IInputService inputService)
        {
            _inputService = inputService;

            _inputService.HandlePressed += OnHandlePressed;
            _inputService.HandleMoved += OnHandleMoved;
            _inputService.AimingButtonPressed += OnAimingButtonPressed;
        }

        public event Action Shooted;
        public event Action<Vector2> AimShifted;
        public event Action<Vector2> HandlePressed;
        public event Action Aimed;

        public void Dispose()
        {
            _inputService.HandlePressed -= OnHandlePressed;
            _inputService.HandleMoved -= OnHandleMoved;
            _inputService.AimingButtonPressed -= OnAimingButtonPressed;
        }

        private void OnAimingButtonPressed()
        {
            Aimed?.Invoke();
            Shooted?.Invoke();
        }

        private void OnHandleMoved(Vector2 handlePosition)
        {
            AimShifted?.Invoke(handlePosition);
        }

        private void OnHandlePressed(Vector2 handlePosition)
        {
            HandlePressed?.Invoke(handlePosition);
        }
    }
}