using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Player.DronePlayer
{
    [RequireComponent(typeof(Rigidbody))]
    public class DroneMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _maxBankAngle = 40f;
        [SerializeField] private float _bankSmoothness = 10f;
        [SerializeField] private float _bankReturnSpeed = 10f;

        [SerializeField] private Rigidbody _rigidbody;

        private float _currentBankAngle;
        private float _targetBankAngle;
        private Coroutine _rotationRoutine;

        public float CurrentBankAngle => _currentBankAngle;

        public void HandleRotation(Vector2 rotation)
        {
            CalculateBanking(rotation);
            ApplyRotation(rotation);
            ApplyMovement();
        }

        public void StartInitialRotation(Vector2 startRotation, Vector2 targetRotation, float duration)
        {
            if (_rotationRoutine != null)
                StopCoroutine(_rotationRoutine);

            _rotationRoutine = StartCoroutine(RotateToInitialPosition(startRotation, targetRotation, duration));
        }

        private IEnumerator RotateToInitialPosition(Vector2 startRotation, Vector2 targetRotation, float duration)
        {
            float progress;
            float passedTime = 0;

            while (passedTime < duration)
            {
                progress = passedTime / duration;
                passedTime += Time.deltaTime;

                Vector2 currentRotation = Vector2.Lerp(startRotation, targetRotation, progress);
                CalculateBanking(currentRotation);
                ApplyRotation(currentRotation);

                yield return null;
            }
        }

        private void CalculateBanking(Vector2 rotation)
        {
            float yRotationDelta = rotation.y - transform.rotation.eulerAngles.y;
            _targetBankAngle = Mathf.Clamp(-yRotationDelta * _maxBankAngle * 10f, -_maxBankAngle, _maxBankAngle);
            _currentBankAngle = Mathf.Lerp(
                _currentBankAngle,
                _targetBankAngle,
                (_targetBankAngle == 0 ? _bankReturnSpeed : _bankSmoothness) * Time.deltaTime
            );
        }

        private void ApplyRotation(Vector2 rotation)
        {
            transform.rotation = Quaternion.Euler(rotation.x, rotation.y, _currentBankAngle);
        }

        private void ApplyMovement()
        {
            _rigidbody.velocity = transform.forward * _speed;
        }
    }
}
