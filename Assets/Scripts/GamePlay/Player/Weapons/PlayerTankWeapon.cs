using System;
using System.Collections;
using Assets.Scripts.GamePlay.Camera;
using Assets.Scripts.GamePlay.Player.Aim;
using Assets.Scripts.Infrastructure.Factoris.Bullets;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Services.StaticData.ScriptableConfig;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay.Player.Weapons
{
    public abstract class PlayerTankWeapon : MonoBehaviour, IShootable
    {
        [SerializeField] private uint _bulletsCapacity;
        [SerializeField] private uint _requireShotsNumberToSuperShot;
        [SerializeField] private uint _bulletShootsCount;
        [SerializeField] private float _bulletShootsDuration;
        [SerializeField] private uint _superBulletShootsCount;
        [SerializeField] private float _supperBulletShootsDuration;
        [SerializeField] private AudioSource _audioSource;

        private GameplayCamera _gameplayCamera;
        private TankAiming _aiming;
        private CameraShaking _cameraShaking;
        private GameplaySettingsConfig _gameplaySettings;

        private Transform[] _bulletPoints;

        private uint _shootsNumberToSuperShot;
        private uint _bulletsCount;

        public event Action BulletsCountChanged;
        public event Action<float> Reloaded;

        protected IBulletFactory BulletFactory { get; private set; }
        protected Quaternion BulletRotation => _gameplayCamera.transform.rotation;
        public uint BulletsCount => _bulletsCount;
        public uint RequireShotsNumberToSuperShot => _requireShotsNumberToSuperShot;
        public uint ShootsNumberToSuperShot => _shootsNumberToSuperShot;

        protected uint BulletShootsCount => _bulletShootsCount;
        protected float BulletShootsDuration => _bulletShootsDuration;
        protected uint SuperBulletShootsCount => _superBulletShootsCount;
        protected float SuperBulletShootsDuration => _supperBulletShootsDuration;

        [Inject]
        private void Construct(
            IBulletFactory bulletFactory,
            GameplayCamera gameplayCamera,
            TankAiming aiming,
            CameraShaking cameraShaking,
            IStaticDataService staticDataService)
        {
            BulletFactory = bulletFactory;
            _gameplayCamera = gameplayCamera;
            _aiming = aiming;
            _cameraShaking = cameraShaking;
            _gameplaySettings = staticDataService.GameplaySettingsConfig;

            _shootsNumberToSuperShot = _requireShotsNumberToSuperShot;
            _bulletsCount = _bulletsCapacity;

            _aiming.Shooted += OnShoted;
        }

        public void SetBulletPoints(Transform[] bulletPoints) =>
            _bulletPoints = bulletPoints;

        private void OnDestroy()
        {
            _aiming.Shooted -= OnShoted;
        }

        private void OnShoted()
        {
            if (_bulletsCount == 0)
                return;

            _bulletsCount--;

            if (_shootsNumberToSuperShot == 0)
            {
                _shootsNumberToSuperShot = _requireShotsNumberToSuperShot;

                SuperShoot();
            }
            else
            {
                _shootsNumberToSuperShot--;

                Shoot();
            }

            BulletsCountChanged?.Invoke();

            if (_bulletsCount == 0)
                StartCoroutine(Reloader());
        }

        protected void OnBulletCreated()
        {
            _cameraShaking.Shake();
            _audioSource.Play();
        }

        protected Transform GetBulletPoint(int index)
        {
            int bulletPointIndex = index >= _bulletPoints.Length ? index - ((int)(index / _bulletPoints.Length) * _bulletPoints.Length) : index;

            return _bulletPoints[bulletPointIndex];
        }

        protected abstract void Shoot();

        protected abstract void SuperShoot();

        private IEnumerator Reloader()
        {
            _aiming.Reload();
            Reloaded?.Invoke(_gameplaySettings.ReloadDuration);

            yield return new WaitForSeconds(_gameplaySettings.ReloadDuration);

            _bulletsCount = _bulletsCapacity;
            _aiming.FinishReload();
            BulletsCountChanged.Invoke();
        }
    }
}
