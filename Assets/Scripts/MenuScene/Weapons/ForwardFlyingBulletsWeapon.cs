using Assets.Scripts.Enum;
using Assets.Scripts.Infrastructure.Factoris.Bullets;
using UnityEngine;

namespace Assets.Scripts.MenuScene.Weapons
{
    public class ForwardFlyingBulletsWeapon : Weapon
    {
        [SerializeField] private ForwardFlyingBulletType _bulletType;

        protected override void CreateBullet(IBulletFactory bulletFactory, Vector3 position, Quaternion rotation)
        {
            bulletFactory.CreateForwardFlyingBullet(_bulletType, position, rotation);
        }
    }
}
