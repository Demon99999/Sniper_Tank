using System;
using System.Collections;
using Assets.Scripts.Enum;
using Assets.Scripts.Infrastructure.Factoris.Bullets;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.MenuScene.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private uint _bulletsCount;
        [SerializeField] private float _shootsCooldown;
        [SerializeField] private MuzzleType _muzzleType;
        [SerializeField] private AudioSource _audioSource;

        private Transform[] _bulletPoints;

        private IBulletFactory _bulletFactory;

        public bool IsShooted { get; private set; }

        [Inject]
        private void Construct(IBulletFactory bulletFactory)
        {
            _bulletFactory = bulletFactory;

            IsShooted = false;
        }

        public void SetBulletPoints(Transform[] bulletPoints)
        {
            _bulletPoints = bulletPoints;
        }

        public void Shoot(Action shooted)
        {
            StartCoroutine(Shooter(shooted));
        }

        protected abstract void CreateBullet(IBulletFactory bulletFactory, Vector3 position, Quaternion rotation);

        private IEnumerator Shooter(Action shooted)
        {
            WaitForSeconds cooldown = new WaitForSeconds(_shootsCooldown);
            int bulletPointIndex = 0;

            IsShooted = true;

            for (int i = 0; i < _bulletsCount; i++)
            {
                bulletPointIndex = bulletPointIndex >= _bulletPoints.Length ? 0 : bulletPointIndex;
                Transform bulletPoint = _bulletPoints[bulletPointIndex];

                shooted?.Invoke();

                _bulletFactory.CreateMuzzle(_muzzleType, bulletPoint.position, bulletPoint.rotation);
                CreateBullet(_bulletFactory, bulletPoint.position, bulletPoint.rotation);
                _audioSource.Play();

                bulletPointIndex++;

                yield return cooldown;
            }

            IsShooted = false;
        }
    }
}
