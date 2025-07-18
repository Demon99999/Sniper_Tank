using System.Collections;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Services.StaticData.ScriptableConfig;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay.Destructions
{
    public class DestructionPart : MonoBehaviour
    {
        [SerializeField] private bool _isDestroyedImmediately;

        private DestructionConfig _destructionConfig;

        private Rigidbody _rigidbody;

        protected virtual bool IsIgnoreRotation => false;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _destructionConfig = staticDataService.DestructionConfig;

            _rigidbody = GetDestructionRigidbody();
            _rigidbody.isKinematic = true;
        }

        public virtual void Destruct(Vector3 explosionPosition, uint explosionForce)
        {
            gameObject.layer = _destructionConfig.Layer;

            Vector3 explosionDirection = (transform.position - explosionPosition).normalized;
            explosionDirection += _destructionConfig.AdditionalDestructionDirection;
            explosionDirection.Normalize();

            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(explosionDirection * explosionForce, ForceMode.Impulse);

            if (IsIgnoreRotation == false)
                _rigidbody.AddTorque(explosionDirection * _destructionConfig.RotationForce, ForceMode.Impulse);

            StartCoroutine(Destroyer());
        }

        protected virtual Rigidbody GetDestructionRigidbody()
        {
            return GetComponent<Rigidbody>();
        }

        private IEnumerator Destroyer()
        {
            yield return new WaitForSeconds(_destructionConfig.DestroyDelay);

            if (_isDestroyedImmediately)
                Destroy(gameObject);

            Vector3 targetScale = Vector3.zero;
            Vector3 startScale = transform.localScale;
            float passedTime = 0;
            float progress;

            while (transform.localScale != targetScale)
            {
                progress = passedTime / _destructionConfig.DestroyDuration;
                passedTime += Time.deltaTime;

                transform.localScale = Vector3.Lerp(startScale, targetScale, progress);

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}