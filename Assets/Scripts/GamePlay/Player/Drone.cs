using System;
using Assets.Scripts.GamePlay.Camera;
using Assets.Scripts.GamePlay.Player.DronePlayer;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay.Player
{
    public class Drone : HandleableRotationObject
    {
        [SerializeField] private DroneMovement _movement;
        [SerializeField] private DroneCameraController _cameraController;
        [SerializeField] private DroneEffects _effects;

        [SerializeField] private float _cameraBlendDuration = 1f;
        [SerializeField] private uint _maxRotation = 90;
        [SerializeField] private uint _maxDistance = 100;
        [SerializeField] private bool _lockCursorWhenActive = true;

        private GameplayCamera _gameplayCamera;
        private RotationCamera _rotationCamera;
        private bool _canMove;
        private bool _isExploded;
        private Vector3 _startDirection;
        private Vector3 _startPosition;

        public event Action Exploded;

        [Inject]
        private void Construct(GameplayCamera gameplayCamera, RotationCamera rotationCamera)
        {
            _gameplayCamera = gameplayCamera;
            _rotationCamera = rotationCamera;
        }

        protected override void OnHandlePressed(Vector2 handlePosition)
        {
            if (!_canMove && !_isExploded)
            {
                StartDroneActivation();
            }
            base.OnHandlePressed(handlePosition);
        }

        protected override void OnAimShifted(Vector2 handlePosition)
        {
            if (_canMove && !_isExploded)
            {
                base.OnAimShifted(handlePosition);
                _movement.HandleRotation(Rotation);
                CheckBoundaries();
            }
        }

        protected override Vector2 ClampRotation(Vector2 rotation)
        {
            return rotation;
        }

        private void StartDroneActivation()
        {
            _canMove = true;
            _effects.HideCursor(_lockCursorWhenActive);

            _gameplayCamera.SetBlednDuration(_cameraBlendDuration);
            _cameraController.SetActive(true, _cameraBlendDuration);

            _startDirection = _movement.transform.forward;
            _startPosition = transform.position;

            _effects.PlayActivationSound();
            Rotation = new Vector2(0, _rotationCamera.Rotation.y);
            _movement.StartInitialRotation(Rotation, _rotationCamera.Rotation, _cameraBlendDuration);
        }

        private void CheckBoundaries()
        {
            if (Vector3.Angle(_movement.transform.forward, _startDirection) > _maxRotation ||
                Vector3.Distance(_startPosition, transform.position) > _maxDistance)
            {
                Explode();
            }
        }

        private void Explode()
        {
            _isExploded = true;
            _effects.ShowCursor(_lockCursorWhenActive);
            _cameraController.SetActive(false);
            _gameplayCamera.SetBlednDuration(0);
            _rotationCamera.ResetRotation();
            _effects.PlayExplosion();
            Exploded?.Invoke();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_canMove || _isExploded) return;
            Explode();
        }

        public class Factory : PlaceholderFactory<string, Vector3, Quaternion, UniTask<Drone>>
        {
        }
    }
}