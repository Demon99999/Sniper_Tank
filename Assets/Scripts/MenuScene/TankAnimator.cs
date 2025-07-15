using System.Collections;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Services.StaticData.ScriptableConfig;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.MenuScene
{
    public abstract class TankAnimator : MonoBehaviour
    {
        private Coroutine _animator;

        public AnimationsConfig AnimationsConfig { get; private set; }

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            //AnimationsConfig = staticDataService.AnimationsConfig;
        }

        public void Play()
        {
            if (isActiveAndEnabled == false)
                return;

            if (_animator != null)
                StopCoroutine(_animator);

            _animator = StartCoroutine(Animator());
        }

        protected abstract IEnumerator Animator();
    }
}
