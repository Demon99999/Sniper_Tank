using Assets.Scripts.GamePlay.Handlers;
using Assets.Scripts.GamePlay.Player.Aim;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Ui.Game.Gameplay
{
    public class GameplayWindow : OpenableWindow
    {
        [SerializeField] private CanvasGroup _overviewAimCanvasGroup;
        [SerializeField] private CanvasGroup _aimButtonCanvasGroup;
        [SerializeField] private CanvasGroup _aimingCanvasGroup;
        [SerializeField] private Button _restartWindowButton;
        [SerializeField] private CanvasGroup _restartWindowButtonCanvasGroup;
        [SerializeField] private CanvasGroup _gameplayInfoCanvasGroup;
        [SerializeField] private Button _optionsWindowButton;
        [SerializeField] private CanvasGroup _currengGameInfoCanvasGroup;

        private DefeatHandler _defeatHandler;
        private VictoryHandler _victoryHangler;
        private RestartWindow _restartWindow;
        private IAiming _aiming;
        private OptionsWindow _optionsWindow;

        private bool _isAimButtonHided;
        private bool _isAimed;

        protected CanvasGroup OverviewAimCanvasGroup => _overviewAimCanvasGroup;
        protected CanvasGroup AimingCanvasGroup => _aimingCanvasGroup;

        [Inject]
        private void Construct(
            DefeatHandler defeatHandler,
            VictoryHandler victoryHandler,
            RestartWindow restartWindow,
            IAiming aiming,
            OptionsWindow optionsWindow)
        {
            _defeatHandler = defeatHandler;
            _victoryHangler = victoryHandler;
            _restartWindow = restartWindow;
            _aiming = aiming;
            _optionsWindow = optionsWindow;

            _isAimButtonHided = false;
            _isAimed = false;

            _defeatHandler.Defeated += HideAimButton;
            _defeatHandler.WindowsSwitched += OnWindowsSwithced;
            _defeatHandler.ProgressRecovered += OnProgressRecovery;
            _victoryHangler.WindowsSwithed += OnWindowsSwithced;
            _victoryHangler.Woned += HideAimButton;
            _restartWindowButton.onClick.AddListener(OnRestartWindowButtonClicked);
            _aiming.Aimed += OnAimed;
            _optionsWindowButton.onClick.AddListener(OnOptionsWindowButtonClicked);
        }

        protected virtual void OnDestroy()
        {
            _defeatHandler.Defeated -= HideAimButton;
            _defeatHandler.WindowsSwitched -= OnWindowsSwithced;
            _defeatHandler.ProgressRecovered -= OnProgressRecovery;
            _victoryHangler.WindowsSwithed -= OnWindowsSwithced;
            _victoryHangler.Woned -= HideAimButton;
            _restartWindowButton.onClick.RemoveListener(OnRestartWindowButtonClicked);
            _aiming.Aimed -= OnAimed;
            _optionsWindowButton.onClick.RemoveListener(OnOptionsWindowButtonClicked);
        }

        private void OnProgressRecovery()
        {
            _isAimButtonHided = false;
            SetAimButtonActive(true);
            Show();
        }

        private void HideAimButton() =>
            _isAimButtonHided = true;

        private void OnWindowsSwithced() =>
            Hide();

        private void OnRestartWindowButtonClicked() =>
            _restartWindow.Show();

        private void OnAimed()
        {
            if (_isAimed)
                return;

            _isAimed = true;

            _restartWindowButtonCanvasGroup.alpha = 1;
            _restartWindowButtonCanvasGroup.interactable = true;
            _restartWindowButtonCanvasGroup.blocksRaycasts = true;
            _gameplayInfoCanvasGroup.alpha = 0;
            _currengGameInfoCanvasGroup.alpha = 1;
        }

        private void OnOptionsWindowButtonClicked() =>
            _optionsWindow.Show();

        protected void SetAimButtonActive(bool isActive)
        {
            isActive = _isAimButtonHided ? false : isActive;
            _aimButtonCanvasGroup.alpha = isActive ? 1 : 0;
            _aimButtonCanvasGroup.blocksRaycasts = isActive;
            _aimButtonCanvasGroup.interactable = isActive;
        }
    }
}