using System.Collections;
using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Player.Weapons
{
    public class HomingBulletWeapon : PlayerTankWeapon
    {
        private int _bulletNumber;

        protected override void Shoot()
        {
            BulletFactory.CreateForwardFlyingBullet(ForwardFlyingBulletType.TankRocket,
                GetBulletPoint(_bulletNumber).position, BulletRotation);

            _bulletNumber++;
            OnBulletCreated();
        }

        protected override void SuperShoot()
        {
            StartCoroutine(Shooter(SuperBulletShootsCount, SuperBulletShootsDuration, HomingBulletType.Rocket));
        }

        private IEnumerator Shooter(uint shootsCount, float shootsDuration, HomingBulletType bulletType)
        {
            WaitForSeconds duration = new WaitForSeconds(shootsDuration);

            for (int i = 0; i < shootsCount; i++)
            {
                BulletFactory.CreateHomingBullet(bulletType, GetBulletPoint(i).position, BulletRotation);
                OnBulletCreated();

                yield return duration;
            }
        }
    }
}
