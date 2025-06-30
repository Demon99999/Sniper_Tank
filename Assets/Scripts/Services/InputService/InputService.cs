using System;
using UnityEngine;

namespace Assets.Scripts.Services.InputService
{
    public class InputService : IInputService
    {
        private readonly InputActionSheme _inputActionSheme;

        public InputService()
        {
            _inputActionSheme = new InputActionSheme();

            SetActive(true);

            _inputActionSheme.GameplayInput.AimingButtonPressed.performed += ctx => AimingButtonPressed?.Invoke();
            _inputActionSheme.GameplayInput.UndoAimingButtonPressed.performed += ctx => UndoAimingButtonPressed?.Invoke();

            _inputActionSheme.HandleInput.HandleMove.started += ctx => HandlePressed?.Invoke(ctx.ReadValue<Vector2>());
            _inputActionSheme.HandleInput.HandleMove.performed += ctx => HandleMoved?.Invoke(ctx.ReadValue<Vector2>());
            _inputActionSheme.HandleInput.HandleMove.canceled += ctx => HandleMoveCompleted?.Invoke();
        }

        public event Action AimingButtonPressed;
        public event Action UndoAimingButtonPressed;

        public event Action<Vector2> HandlePressed;
        public event Action<Vector2> HandleMoved;
        public event Action HandleMoveCompleted;

        public void SetActive(bool isActive)
        {
            if (isActive)
            {
                _inputActionSheme.Enable();
            }
            else
            {
                _inputActionSheme.Disable();
            }
        }
    }
}