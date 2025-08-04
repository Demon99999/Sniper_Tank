using UnityEngine;

namespace Assets.Scripts.GamePlay.Bullets
{
    public class CollidingBullet : ExplodingBullet
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private bool _isExplosionAlongMoveDiretion;

        private float _explosionLifeTime;
        private uint _flightSpeed;
        private bool _isExploded;

        private void Start()
        {
            _isExploded = false;
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            Explode();
        }

        public CollidingBullet BindSettings(float explosionLifeTime, uint flightSpeed, float lifeTimeLimit)
        {
            _explosionLifeTime = explosionLifeTime;
            _flightSpeed = flightSpeed;

            ChangeTrajectory();
            DestroyAfterLifeTimeLimit(lifeTimeLimit);

            return this;
        }

        protected virtual void Explode()
        {
            if (_isExploded || this == null) return;

            _isExploded = true;
            Stop();

            var rotation = _isExplosionAlongMoveDiretion && transform.forward != Vector3.zero
                ? Quaternion.LookRotation(-transform.forward)
                : Quaternion.identity;

            CreateExplosionParticle(transform.position, rotation);
            Explode(transform.position);

            if (Projectile != null) Destroy(Projectile);
            Destroy(gameObject, _explosionLifeTime);
        }

        protected void ChangeTrajectory()
        {
            if (_rigidbody != null)
            {
                _rigidbody.velocity = transform.forward * _flightSpeed;
            }
        }

        protected virtual void DestroyAfterLifeTimeLimit(float lifeTimeLimt)
        {
            Destroy(gameObject, lifeTimeLimt);
        }

        private void Destroy()
        {
            Destroy(gameObject, _explosionLifeTime);
        }

        private void DestroyProjectile()
        {
            Destroy(Projectile);
        }

        private void Stop()
        {
            if (_rigidbody != null)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.isKinematic = true;
            }

            if (_collider != null)
            {
                _collider.enabled = false;
            }
        }
    }
}
