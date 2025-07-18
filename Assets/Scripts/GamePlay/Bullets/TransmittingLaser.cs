using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GamePlay.Enemis;
using Assets.Scripts.GamePlay.Handlers;
using Assets.Scripts.Infrastructure.Factoris.Bullets;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay.Bullets
{
    public class TransmittingLaser : DirectionalLaser
    {
        private readonly Vector3 _offset = new Vector3(0, 1, 0);

        private IBulletFactory _bulletFactory;
        private VictoryHandler _victoryHandler;

        [Inject]
        private void Construct(IBulletFactory bulletFactory, VictoryHandler victoryHandler)
        {
            _bulletFactory = bulletFactory;
            _victoryHandler = victoryHandler;
        }

        public TransmittingLaser BindTargetsCount(int targetsCount)
        {
            CreateLaser(targetsCount);

            return this;
        }

        private async void CreateLaser(int targetsCount)
        {
            if (Launch())
            {
                IReadOnlyList<Enemy> enemies = _victoryHandler.Enemies;

                enemies = enemies.Where(enemy => enemy.IsDestructed == false).OrderBy(enemy => Vector3.Distance(HitInfo.point, enemy.transform.position)).Take(targetsCount).ToArray();

                if (enemies.Count == 0)
                    return;

                Vector3 fitsPoint = HitInfo.point;

                if (HitInfo.transform.TryGetComponent(out Enemy _))
                    enemies = enemies.Skip(1).ToArray();

                await _bulletFactory.CreateTargetingLaser(fitsPoint, enemies[0].transform.position + _offset);

                for (int i = 0; i < enemies.Count - 1; i++)
                    await _bulletFactory.CreateTargetingLaser(enemies[i].transform.position + _offset, enemies[i + 1].transform.position + _offset);
            }
        }
    }
}
