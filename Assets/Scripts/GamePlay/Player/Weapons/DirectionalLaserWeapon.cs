using System.Collections;
using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Player.Weapons
{
    public class DirectionalLaserWeapon : PlayerTankWeapon
    {
        protected override void Shoot()
        {
            BulletFactory.CreateHomingBullet(HomingBulletType.Laser, GetBulletPoint(0).position, BulletRotation);
            OnBulletCreated();
        }

        protected override void SuperShoot()
        {
            StartCoroutine(Shooter(SuperBulletShootsCount, SuperBulletShootsDuration));
        }

        private IEnumerator Shooter(uint shootsCount, float shootsDuration)
        {
            WaitForSeconds duration = new WaitForSeconds(shootsDuration);

            for (int i = 0; i < shootsCount; i++)
            {
                BulletFactory.CreateHomingBullet(HomingBulletType.Laser, GetBulletPoint(i).position, BulletRotation);
                OnBulletCreated();

                yield return duration;
            }
        }
    }
}
