using System;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class Explosion : MonoBehaviour
    {
        private const float CollidingDelat = 0.4f;

        private readonly Collider[] _overlapColliders = new Collider[32];

        [SerializeField] private ParticleSystem _explosionParticlePrefab;
        //[SerializeField] private AudioSource _audioSource;

        public event Action Exploded;

        protected void CreateExplosionParticle(Vector3 position, Quaternion rotation)
        {
            ParticleSystem explosionParticle = Instantiate(_explosionParticlePrefab, position, rotation, transform);
            explosionParticle.Play();
        }

        protected void Explode(Vector3 position, float explosionRadius, uint explosionForce, uint damage)
        {
            int overlapCount = Physics.OverlapSphereNonAlloc(position, explosionRadius, _overlapColliders);

            for (int i = 0; i < overlapCount; i++)
            {
                if (_overlapColliders[i].TryGetComponent(out IDamageable damageable))
                {
                    bool isDamageableCollided = Vector3.Distance(position, _overlapColliders[i].ClosestPoint(position)) < CollidingDelat;
                    damageable.TakeDamage(new ExplosionInfo(position, explosionForce, isDamageableCollided, damage));
                    //_audioSource.Play();
                }
            }

            Exploded?.Invoke();
        }
    }
}