using System.Collections;
using Assets.Scripts.GamePlay.Enemis.Robot;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Enemis.EnemyShooting
{
    public class EnemyRobotShooting : EnemyShooting
    {
        private const float AngleDelta = 6;
        private const float ChangingRotationDuration = 0.4f;
        private const float LaserRotationSpeed = 10;
        private const float RaycasDistance = 300;

        [SerializeField] private RobotLaser _laserPrefab;
        [SerializeField] private uint _damgagePerTime;
        [SerializeField] private float _damageCooldown;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private uint _explosionForce;
        [SerializeField] private EnemyRobot _enemyRobot;

        private RobotLaser _laser;
        private Quaternion _targetRotation;
        private float _attackPassedTime;
        private float _diretionChangingPassedTime;

        protected override Vector3 GetCurrentShootingPosition() =>
            _shootPoint.position;

        protected override IEnumerator Shooter()
        {
            while (IsShooted)
            {
                if (_enemyRobot.IsStopped)
                {
                    yield return HandleLaserAttack();
                }
                else
                {
                    CleanUpLaser();
                }
                yield return null;
            }
        }

        private IEnumerator HandleLaserAttack()
        {
            if (IsPlayerInAttackAngle())
            {
                UpdateTimers();
                HandleLaser();
                TryDealDamage();
            }
            else
            {
                CleanUpLaser();
            }
            yield return null;
        }

        private bool IsPlayerInAttackAngle()
        {
            Vector3 shootPointForward = _shootPoint.forward;
            Vector3 targetDirection = (PlayerWrapper.transform.position - _shootPoint.position).normalized;

            shootPointForward.y = 0;
            targetDirection.y = 0;

            return Vector3.Angle(shootPointForward, targetDirection) < AngleDelta;
        }

        private void UpdateTimers()
        {
            _diretionChangingPassedTime += Time.deltaTime;
            _attackPassedTime += Time.deltaTime;
        }

        private void HandleLaser()
        {
            if (_laser == null)
            {
                InitializeLaser();
            }
            else
            {
                RotateLaser();
            }

            if (_diretionChangingPassedTime >= ChangingRotationDuration)
            {
                _diretionChangingPassedTime = 0;
                _targetRotation = GetShootingRotation();
            }

            UpdateLaserPosition();
        }

        private void InitializeLaser()
        {
            _laser = Instantiate(_laserPrefab, _shootPoint.position, GetShootingRotation(), _shootPoint.transform);
            _laser.transform.localPosition = Vector3.zero;
            AudioSource.Play();
        }

        private void RotateLaser()
        {
            _laser.transform.rotation = Quaternion.RotateTowards(
                _laser.transform.rotation,
                _targetRotation,
                LaserRotationSpeed * Time.deltaTime
            );
        }

        private void UpdateLaserPosition()
        {
            bool isHitted = Physics.Raycast(
                GetCurrentShootingPosition(),
                _laser.transform.forward,
                out RaycastHit hitInfo,
                RaycasDistance
            );

            _laser.SetLaser(GetCurrentShootingPosition(), hitInfo.point);
        }

        private void TryDealDamage()
        {
            if (_attackPassedTime >= _damageCooldown)
            {
                _attackPassedTime = 0;
                PerformDamage();
            }
        }

        private void PerformDamage()
        {
            if (Physics.Raycast(
                GetCurrentShootingPosition(),
                _laser.transform.forward,
                out RaycastHit hitInfo,
                RaycasDistance) &&
                hitInfo.transform.TryGetComponent(out IDamageable damageable) &&
                damageable is not EnemyRobot)
            {
                damageable.TakeDamage(new ExplosionInfo(
                    hitInfo.point,
                    _explosionForce,
                    true,
                    _damgagePerTime
                ));
            }
        }

        private void CleanUpLaser()
        {
            if (_laser != null)
            {
                AudioSource.Stop();
                Destroy(_laser.gameObject);
                _laser = null;
            }
        }
    }
}