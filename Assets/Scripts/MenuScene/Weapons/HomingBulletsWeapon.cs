using Assets.Scripts.Enum;
using Assets.Scripts.Infrastructure.Factoris.Bullets;
using UnityEngine;

namespace Assets.Scripts.MenuScene.Weapons
{
    public class HomingBulletsWeapon : Weapon
    {
        protected override void CreateBullet(IBulletFactory bulletFactory, Vector3 position, Quaternion rotation)
        {
            bulletFactory.CreateHomingBullet(HomingBulletType.Laser, position, rotation);
        }
    }
}
