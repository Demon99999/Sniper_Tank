using Assets.Scripts.Enum;
using Assets.Scripts.Infrastructure.Factoris.Bullets;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay.Bullets
{
    public class CompositeBullet : CollidingBullet
    {
        private readonly Vector3 BombDiretionOffset = new Vector3(0, 2, 0);

        private IBulletFactory _bulletFactory;
        private uint _bombsCount;

        [Inject]
        private void Construct(IBulletFactory bulletFactory)
        {
            _bulletFactory = bulletFactory;
        }

        public CompositeBullet BindBombsCount(uint bombsCount)
        {
            _bombsCount = bombsCount;

            return this;
        }

        protected override void Explode()
        {
            base.Explode();
            CreateBombs();
        }

        private void CreateBombs()
        {
            for (int i = 0; i < _bombsCount; i++)
            {
                Vector3 bombDirection = (Random.onUnitSphere + BombDiretionOffset).normalized;

                _bulletFactory.CreateForwardFlyingBullet(ForwardFlyingBulletType.Bomb, transform.position, Quaternion.LookRotation(bombDirection));
            }
        }
    }
}
