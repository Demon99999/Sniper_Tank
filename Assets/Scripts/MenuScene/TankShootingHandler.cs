using System;
using Assets.Scripts.Services.InputService;
using UnityEngine;

namespace Assets.Scripts.MenuScene
{
    public class TankShootingHandler : IDisposable
    {
        private readonly IInputService _inputService;

        private bool _isActive;

        public TankShootingHandler(IInputService inputService)
        {
            _inputService = inputService;

            _isActive = true;

            _inputService.HandlePressed += OnHandlePressed;
        }

        public event Action<Vector2> HandlePressed;

        public void Dispose()
        {
            _inputService.HandlePressed -= OnHandlePressed;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        private void OnHandlePressed(Vector2 handlePosition)
        {
            if (_isActive)
            {
                HandlePressed?.Invoke(handlePosition);
            }
        }
    }
}
