using Assets.Scripts.Enum;

namespace Assets.Scripts.GamePlay.Player.Weapons
{
    public class TransmittingLaserWeapon : PlayerTankWeapon
    {
        protected override void Shoot()
        {
            BulletFactory.CreateHomingBullet(HomingBulletType.Laser, GetBulletPoint(0).position, BulletRotation);
            OnBulletCreated();
        }

        protected override void SuperShoot()
        {
            BulletFactory.CreateTransmittingLaser(GetBulletPoint(0).position, BulletRotation);
            OnBulletCreated();
        }
    }
}
