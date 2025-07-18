using Assets.Scripts.Infrastructure.Factoris.Bullets;
using UnityEngine;

namespace Assets.Scripts.MenuScene.Weapons
{
    public class CompostieBulletsWeapon : Weapon
    {
        protected override void CreateBullet(IBulletFactory bulletFactory, Vector3 position, Quaternion rotation)
        {
            bulletFactory.CreateCompositeBullet(position, rotation);
        }
    }
}
