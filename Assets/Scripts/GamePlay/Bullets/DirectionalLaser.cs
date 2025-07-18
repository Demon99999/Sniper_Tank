using UnityEngine;

namespace Assets.Scripts.GamePlay.Bullets
{
    public class DirectionalLaser : Laser
    {
        private const float MaxDistance = 200;
        private const float Size = 0.5f;

        [SerializeField] private LaserLine _laserLine;

        private RaycastHit _hitInfo;

        protected RaycastHit HitInfo => _hitInfo;

        public override Laser BindLifeTimes(float explosionLifeTime, float projectileLifeTime)
        {
            Launch();

            return base.BindLifeTimes(explosionLifeTime, projectileLifeTime);
        }

        protected bool Launch()
        {
            bool isHited = Physics.Raycast(transform.position, transform.forward, out _hitInfo, MaxDistance);

            _laserLine.Initialize(transform.position, _hitInfo.point, Size);
            _laserLine.SetActive(true);

            if (isHited)
            {
                CreateExplosionParticle(_hitInfo.point, Quaternion.LookRotation(_hitInfo.normal, transform.forward));
                Explode(_hitInfo.point);
            }

            return isHited;
        }
    }
}
