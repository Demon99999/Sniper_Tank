using System.Collections;
using Assets.Scripts.GamePlay.Player.Wrappers;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Services.StaticData.ScriptableConfig;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Ui.Game.Aim
{
    public class TankAimCell : MonoBehaviour
    {
        [SerializeField] private Vector2 _reactionPosition;
        [SerializeField] private CanvasGroup _canvasGroup;

        private PlayerTankWrapper _playerTankWrapper;
        private AnimationsConfig _animationConfig;

        private Coroutine _animator;

        [Inject]
        private void Construct(PlayerTankWrapper playerTankWrapper, IStaticDataService staticDataService)
        {
            _playerTankWrapper = playerTankWrapper;
            _animationConfig = staticDataService.AnimationsConfig;

            _playerTankWrapper.Attacked += OnPlayerAttacked;
        }

        private void OnDestroy()
        {
            _playerTankWrapper.Attacked -= OnPlayerAttacked;
        }

        private void OnPlayerAttacked(Vector2 attackPosition)
        {
            if (attackPosition != _reactionPosition)
                return;

            if (_animator != null)
                StopCoroutine(_animator);

            _animator = StartCoroutine(Animator());
        }

        private IEnumerator Animator()
        {
            float progess;
            float passedTime = 0;
            float duration = _animationConfig.TankAimCellAnimationDuration;
            AnimationCurve animationCurve = _animationConfig.TankAimCellAnimationCurve;

            while (passedTime <= duration)
            {
                progess = passedTime / duration;
                passedTime += Time.deltaTime;

                _canvasGroup.alpha = animationCurve.Evaluate(progess);

                yield return null;
            }
        }
    }
}
