using System.Collections;
using System.Linq;
using Assets.Scripts.GamePlay.Camera;
using Assets.Scripts.GamePlay.Enemis;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay.Bullets
{
    public class HomingBullet : CollidingBullet
    {
        private const float Offset = 1;

        private GameplayCamera _gameplayCamera;

        private uint _rotationSpeed;
        private float _targetingDelay;
        private uint _searchRadius;

        protected Transform Target;
        private bool _isFollowed;

        [Inject]
        private void Construct(GameplayCamera gameplayCamera)
        {
            _gameplayCamera = gameplayCamera;
        }

        private void Start()
        {
            SearchTarget(_searchRadius);
        }

        public HomingBullet BindHomingSettings(uint searchRadius, uint rotationSpeed, float targetingDelay)
        {
            _rotationSpeed = rotationSpeed;
            _targetingDelay = targetingDelay;
            _searchRadius = searchRadius;

            _isFollowed = false;

            return this;
        }

        protected override void Explode()
        {
            base.Explode();
            _isFollowed = false;
        }

        protected virtual void SearchTarget(uint searchRadius)
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();

            Vector3 center = _gameplayCamera.Camera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, 0));

            enemies = enemies.Where(enemy => Vector3.Distance(center, _gameplayCamera.Camera.WorldToScreenPoint(new Vector3(enemy.transform.position.x, enemy.transform.position.y, 1))) <= searchRadius).ToArray();

            if (enemies.Length > 0)
            {
                Target = enemies[Random.Range(0, enemies.Length)].transform;

                StartCoroutine(TargetLocator());
            }
        }

        protected IEnumerator TargetLocator()
        {
            _isFollowed = true;

            yield return new WaitForSeconds(_targetingDelay);

            while (_isFollowed && Target != null)
            {
                Vector3 direction = (Target.transform.position + (Vector3.up * Offset) - transform.position);

                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                    ChangeTrajectory();
                }

                yield return null;
            }
        }
    }
}
