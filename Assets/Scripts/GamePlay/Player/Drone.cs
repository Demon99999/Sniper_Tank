using System;
using System.Collections;
using Assets.Scripts.GamePlay.Camera;
using Assets.Scripts.GamePlay.Destructions;
using Assets.Scripts.GamePlay.Player.Aim;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Assets.Scripts.GamePlay.Player
{
    public class Drone : HandleableRotationObject
    {
        private const string MixerVolume = "Volume";
        private const int ActiveCameraPriority = 1;
        private const int DeactiveCameraPriority = 0;

        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _cameraBlendDuration;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed;
        [SerializeField] private DroneExplosion _explosion;
        [SerializeField] private GameObject _drone;
        [SerializeField] private Collider _collider;
        [SerializeField] private uint _maxRotation;
        [SerializeField] private uint _maxDistance;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private float _soundVolume;
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private float _maxBankAngle = 40f;
        [SerializeField] private float _bankSmoothness = 10f;
        [SerializeField] private float _bankReturnSpeed = 10f;

        [SerializeField] private bool _lockCursorWhenActive = true;

        private float _targetBankAngle;
        private float _currentBankAngle;
        private float _previousYRotation;

        private DroneAiming _aiming;
        private GameplayCamera _gameplayCamera;
        private RotationCamera _rotationCamera;

        private bool _canMove;
        private bool _isCollided;
        private bool _isExploded;
        private bool _isShootedProcess;

        private Vector3 _startDiretion;
        private Vector3 _startPosition;

        private float _startSoundVolume;

        public event Action Exploded;


        [SerializeField] private float _cameraBankMultiplier = 0.6f;
        [SerializeField] private float _cameraBankSmoothness = 3f;

        private Quaternion _cameraBaseRotation;
        private float _currentCameraBankAngle;

        [Inject]
        private void Construct(DroneAiming droneAiming, GameplayCamera gameplayCamera, RotationCamera rotationCamera)
        {
            _aiming = droneAiming;
            _gameplayCamera = gameplayCamera;
            _rotationCamera = rotationCamera;

            _canMove = false;
            _isCollided = false;
            _isExploded = false;
            _isShootedProcess = false;

            _aiming.Shooted += OnPlayerShooted;

            _cameraBaseRotation = _camera.transform.localRotation;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _aiming.Shooted -= OnPlayerShooted;
            ShowCursor();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isCollided || _isExploded)
                return;

            _isCollided = true;
            Explode();
        }

        private void Update()
        {
            if (_canMove == false || _isExploded)
                return;

            if (Vector3.Angle(_rigidbody.transform.forward, _startDiretion) > _maxRotation
                || Vector3.Distance(_startPosition, transform.position) > _maxDistance)
                Explode();
        }

        private void ShowCursor()
        {
            if (!_lockCursorWhenActive) return;

            //Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void HideCursor()
        {
            if (!_lockCursorWhenActive) return;

            //Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        protected override Vector2 ClampRotation(Vector2 rotation)
        {
            return new Vector2(
                Mathf.Clamp(rotation.x, -85f, 85f),
                rotation.y
            );
        }

        protected override void OnAimShifted(Vector2 handlePosition)
        {
            if (_canMove)
            {
                base.OnAimShifted(handlePosition);
                _rigidbody.velocity = _rigidbody.transform.forward * _speed;
            }
        }

        private void OnPlayerShooted()
        {
            if (_isShootedProcess)
                return;

            _isShootedProcess = true;
            HideCursor();
            _gameplayCamera.SetBlednDuration(_cameraBlendDuration);
            _camera.Priority = ActiveCameraPriority;

            _startDiretion = _rigidbody.transform.forward;
            _startPosition = transform.position;

            _audioMixer.GetFloat(MixerVolume, out _startSoundVolume);
            _audioMixer.SetFloat(MixerVolume, _soundVolume);

            Rotation = new Vector2(0, _rotationCamera.Rotation.y);

            StartCoroutine(Rotater());
        }

        private void Explode()
        {
            _isExploded = true;
            ShowCursor();
            _camera.Priority = DeactiveCameraPriority;
            _audioMixer.SetFloat(MixerVolume, _startSoundVolume);
            _gameplayCamera.SetBlednDuration(0);
            _rotationCamera.ResetRotation();
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            _explosion.Explode();
            Destroy(_drone);
            _audioSource.Stop();
            Exploded?.Invoke();
        }

        protected override void Rotate()
        {
            float yRotationDelta = Rotation.y - _previousYRotation;
            _previousYRotation = Rotation.y;

            _targetBankAngle = Mathf.Clamp(-yRotationDelta * _maxBankAngle * 10f, -_maxBankAngle, _maxBankAngle);

            _currentBankAngle = Mathf.Lerp(
                _currentBankAngle,
                _targetBankAngle,
                (_targetBankAngle == 0 ? _bankReturnSpeed : _bankSmoothness) * Time.deltaTime
            );

            transform.rotation = Quaternion.Euler(Rotation.x, Rotation.y, _currentBankAngle);

            UpdateCameraBanking();
        }

        private void UpdateCameraBanking()
        {
            if (_camera == null) return;

            float targetCameraBank = _currentBankAngle * _cameraBankMultiplier;

            _currentCameraBankAngle = Mathf.Lerp(
                _currentCameraBankAngle,
                targetCameraBank,
                _cameraBankSmoothness * Time.deltaTime
            );

            _camera.transform.localRotation = _cameraBaseRotation * Quaternion.Euler(0, 0, _currentCameraBankAngle);
        }

        private IEnumerator Rotater()
        {
            float progress;
            float passedTime = 0;

            Vector2 targetRotation = _rotationCamera.Rotation;
            Vector2 startRotation = Rotation;

            while (Rotation != targetRotation)
            {
                progress = passedTime / _cameraBlendDuration;
                passedTime += Time.deltaTime;

                Rotation = Vector2.Lerp(startRotation, targetRotation, progress);
                Rotate();

                yield return null;
            }

            
            _canMove = true;
            _isShootedProcess = false;
            _rigidbody.velocity = _rigidbody.transform.forward * _speed;
        }

        public class Factory : PlaceholderFactory<string, Vector3, Quaternion, UniTask<Drone>>
        {
        }
    }
}