using UnityEngine;

namespace Assets.Scripts.GamePlay.Destructions
{
    public class ExplosionBarrel : EnemyEngineryExplosion, IDamageable
    {
        [SerializeField] private GameObject _barrel;
        [SerializeField] private AudioSource _audioSource;

        private bool _isExplosded;

        private void Start() =>
            _isExplosded = false;

        public void TakeDamage(ExplosionInfo explosionInfo)
        {
            if (_isExplosded)
                return;

            _isExplosded = true;

            Destroy(_barrel);
            _audioSource.Play();
            Explode();
        }
    }
}
