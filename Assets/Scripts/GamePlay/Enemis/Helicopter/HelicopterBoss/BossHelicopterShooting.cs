using System.Collections;
using Assets.Scripts.GamePlay.Destructions;
using Assets.Scripts.GamePlay.Enemis.EnemyShooting;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Enemis.Helicopter.HelicopterBoss
{
    public class BossHelicopterShooting : EnemyForwartFlyingBulletsShooting
    {
        [SerializeField] private float _shootDuration;
        [SerializeField] private Transform _bulletPoint;
        [SerializeField] private Transform _helicopter;
        [SerializeField] private HelicopterDestructionPart _weaponPart;

        protected override bool CanShoot => 
            _weaponPart.IsDesturcted == false && 
            base.CanShoot && 
            Vector2.Angle(new Vector2(PlayerWrapper.transform.position.x - LookStartPosition.x,
                PlayerWrapper.transform.position.z - LookStartPosition.z),
                new Vector2(_helicopter.transform.forward.x, _helicopter.transform.forward.z)) < AngleDelta;

        protected override Vector3 LookStartPosition => transform.position;

        protected override Vector3 GetCurrentShootPointPosition() =>
            _bulletPoint.position;

        protected override void Shoot()
        {
            StartCoroutine(Waiter());
        }

        private IEnumerator Waiter()
        {
            yield return new WaitForSeconds(_shootDuration);

            base.Shoot();
        }
    }
}