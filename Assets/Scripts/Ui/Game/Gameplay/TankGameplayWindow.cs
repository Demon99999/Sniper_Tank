using System.Collections;
using Assets.Scripts.GamePlay.Player.Aim;
using Assets.Scripts.GamePlay.Player.Weapons;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Ui.Game.Gameplay
{
    public class TankGameplayWindow : GameplayWindow
    {
        [SerializeField] private ReloadPanel _reloadProgressBar;
        [SerializeField] private CanvasGroup _gameInfoCanvasGroup;
        [SerializeField] private CanvasGroup _optionsButton;
        [SerializeField] private CanvasGroup _restartButton;

        private TankAiming _aiming;
        private PlayerTankWeapon _playerTankWeapon;

        private Coroutine _aimChanger;
        private bool _isReloaded;

        [Inject]
        private void Construct(TankAiming tankAiming, PlayerTankWeapon playerTankWeapon)
        {
            _aiming = tankAiming;
            _playerTankWeapon = playerTankWeapon;

            _isReloaded = false;

            _aiming.StateChanged += OnAimingStateChanged;
            _aiming.StateChangingFinished += OnAimingStageChangingFinished;
            _playerTankWeapon.Reloaded += OnReloaded;
        }

        protected override void OnDestroy()
        {
            _aiming.StateChanged -= OnAimingStateChanged;
            _aiming.StateChangingFinished -= OnAimingStageChangingFinished;
            _playerTankWeapon.Reloaded -= OnReloaded;
            base.OnDestroy();
        }

        private void OnAimingStateChanged(bool isAimed, float duration)
        {
            if (isAimed)
            {
                OverviewAimCanvasGroup.alpha = 0;
                _gameInfoCanvasGroup.alpha = 0;
                _restartButton.alpha = 0;
                _optionsButton.alpha = 0;
                SetAimButtonActive(false);
            }
            else
            {
                OverviewAimCanvasGroup.alpha = 1;
                _gameInfoCanvasGroup.alpha = 1;
                _optionsButton.alpha = 1;
                _restartButton.alpha = 1;
            }

            if (_aimChanger != null)
            {
                StopCoroutine(_aimChanger);
            }

            _aimChanger = StartCoroutine(AimChanger(isAimed, duration));
        }

        private void OnAimingStageChangingFinished(bool isAimed)
        {
            if (isAimed == false && _isReloaded == false)
            {
                SetAimButtonActive(true);
            }
        }

        private void OnReloaded(float duration)
        {
            SetAimButtonActive(false);
            _isReloaded = true;

            _reloadProgressBar.StartReload(duration, callback: () =>
            {
                _isReloaded = false;
                SetAimButtonActive(true);
            });
        }

        private IEnumerator AimChanger(bool isAimed, float duration)
        {
            float progress;
            float passedTime = 0;
            float targetAlpha = isAimed ? 1 : 0;
            float startAlpha = AimingCanvasGroup.alpha;

            AimingCanvasGroup.interactable = isAimed;
            AimingCanvasGroup.blocksRaycasts = isAimed;

            while (AimingCanvasGroup.alpha != targetAlpha)
            {
                progress = passedTime / duration;
                passedTime += Time.deltaTime;

                AimingCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);

                yield return null;
            }
        }
    }
}