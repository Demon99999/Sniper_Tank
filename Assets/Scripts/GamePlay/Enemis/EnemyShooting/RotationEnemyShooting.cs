using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Enemis.EnemyShooting
{
    public class RotationEnemyShooting : EnemyForwartFlyingBulletsShooting
    {
        [SerializeField] protected Transform _shootPoint;
        [SerializeField] protected float _rotationSpeed;

        protected bool _isRotating;
        protected bool _isTurnedToTarget;

        protected const float AngleDelta = 5f;

        protected override bool CanShoot => base.CanShoot && _isTurnedToTarget;
        protected override Vector3 LookStartPosition => _shootPoint.position;

        protected virtual void StartRotation()
        {
            _isRotating = true;
            _isTurnedToTarget = false;
        }

        protected virtual void StopRotation()
        {
            _isRotating = false;
        }

        protected override void StartShooting()
        {
            base.StartShooting();
            StartCoroutine(RotateTowardsTarget(PlayerWrapper.transform, transform, _rotationSpeed,
                () => _isTurnedToTarget = true, () => _isTurnedToTarget = false));
        }

        protected override void OnEnemyDestructed()
        {
            base.OnEnemyDestructed();
            StopRotation();
        }

        protected override Vector3 GetCurrentShootPointPosition() => _shootPoint.position;

        protected IEnumerator RotateTowardsTarget(Transform target, Transform rotatingPart, float speed,
            Action onTargetReached, Action onTargetNotReached)
        {
            StartRotation();

            while (_isRotating)
            {
                Vector3 targetDirection = (target.position - rotatingPart.position).normalized;
                Quaternion targetRotation = GetTargetRotation(targetDirection, rotatingPart);

                if (Vector3.Angle(rotatingPart.forward, targetDirection) > AngleDelta)
                {
                    rotatingPart.rotation = Quaternion.RotateTowards(
                        rotatingPart.rotation,
                        targetRotation,
                        speed * Time.deltaTime);

                    onTargetNotReached?.Invoke();
                }
                else
                {
                    onTargetReached?.Invoke();
                }

                yield return null;
            }
        }

        protected virtual Quaternion GetTargetRotation(Vector3 targetDirection, Transform rotatingPart)
        {
            Vector3 shootPointForward = _shootPoint.forward;
            return Quaternion.Euler(
                0,
                rotatingPart.rotation.eulerAngles.y + Quaternion.FromToRotation(shootPointForward,
                    targetDirection).eulerAngles.y,
                0);
        }
    }
}
