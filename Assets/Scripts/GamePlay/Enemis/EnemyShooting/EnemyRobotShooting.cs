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

        protected override Vector3 GetCurrentShootingPosition() =>
            _shootPoint.position;

        protected override IEnumerator Shooter()
        {
            float attackPassedTime = 0;
            float diretionChangingPassedTime = 0;

            while (IsShooted)
            {
                if (_enemyRobot.IsStopped)
                {
                    Vector3 shootPointForward = _shootPoint.forward;
                    Vector3 targetDirection = (PlayerWrapper.transform.position - _shootPoint.position).normalized;

                    shootPointForward.y = 0;
                    targetDirection.y = 0;

                    if (Vector3.Angle(shootPointForward, targetDirection) < AngleDelta)
                    {
                        diretionChangingPassedTime += Time.deltaTime;
                        attackPassedTime += Time.deltaTime;

                        if (_laser == null)
                        {
                            _laser = Instantiate(_laserPrefab, _shootPoint.position, GetShootingRotation(), _shootPoint.transform);
                            _laser.transform.localPosition = Vector3.zero;
                            AudioSource.Play();
                        }
                        else
                        {
                            _laser.transform.rotation = Quaternion.RotateTowards(_laser.transform.rotation, _targetRotation, LaserRotationSpeed * Time.deltaTime);
                        }

                        if (diretionChangingPassedTime >= ChangingRotationDuration)
                        {
                            diretionChangingPassedTime = 0;
                            _targetRotation = GetShootingRotation();
                        }

                        bool isHitted = Physics.Raycast(GetCurrentShootingPosition(), _laser.transform.forward, out RaycastHit hitInfo, RaycasDistance);

                        //_laser.SetLaser(GetCurrentShootingPosition(), hitInfo.point);

                        if (attackPassedTime >= _damageCooldown)
                        {
                            attackPassedTime = 0;

                            if (isHitted && hitInfo.transform.TryGetComponent(out IDamageable damageable) && damageable is not EnemyRobot)
                                damageable.TakeDamage(new ExplosionInfo(hitInfo.point, _explosionForce, true, _damgagePerTime));
                        }
                    }
                    else if (_laser != null)
                    {
                        AudioSource.Stop();
                        Destroy(_laser.gameObject);
                        _laser = null;
                    }
                }
                else if (_laser != null)
                {
                    AudioSource.Stop();
                    Destroy(_laser.gameObject);
                    _laser = null;
                }

                yield return null;
            }
        }
    }
}