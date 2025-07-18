using System;
using System.Collections;
using Assets.Scripts.Enum;
using Assets.Scripts.GamePlay;
using Assets.Scripts.GamePlay.Handlers;
using Assets.Scripts.Services.PersistentProgressServices;
using Assets.Scripts.Services.SaveLoadProgressServices;
using Assets.Scripts.Services.StateMachine;
using Assets.Scripts.Services.StateMachine.States;
using Assets.Scripts.Services.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Ui.Game.VictoryWindow
{
    public class VictoryWindow : OpenableWindow
    {
        private const float CloseDelay = 0.5f;

        [SerializeField] private Button _continueButton;
        [SerializeField] private TMP_Text _rewardValue;
        [SerializeField] private TMP_Text _currentLevelValue;
        [SerializeField] private WalletPanel _walletPanel;
        [SerializeField] private MoneyEffect _moneyEffect;

        private VictoryHandler _wictoryHandler;
        private GameStateMachine _gameStateMachine;
        private GameplayLoadingCurtain _loadingCurtain;
        private IStaticDataService _staticDataService;
        private IPersistentProgressService _persistentProgressService;
        private RewardCounter _rewardCounter;
        private ISaveLoadService _saveLoadService;

        private uint _reward;
        private bool _isContinueButtonClicked;

        protected RewardCounter RewardCounter => _rewardCounter;
        protected IPersistentProgressService PersistentProgressService => _persistentProgressService;

        [Inject]
        private void Construct(
            VictoryHandler wictoryHandler,
            GameStateMachine gameStateMachine,
            GameplayLoadingCurtain loadingCurtain,
            IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService,
            RewardCounter rewardCounter,
            ISaveLoadService saveLoadService)
        {
            _wictoryHandler = wictoryHandler;
            _gameStateMachine = gameStateMachine;
            _loadingCurtain = loadingCurtain;
            _staticDataService = staticDataService;
            _persistentProgressService = persistentProgressService;
            _rewardCounter = rewardCounter;
            _saveLoadService = saveLoadService;

            _currentLevelValue.text = (_persistentProgressService.Progress.CurrentLevelIndex + 1).ToString();

            _isContinueButtonClicked = false;

            _wictoryHandler.WindowsSwithed += OnWindowsSwitched;
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        protected virtual void OnDestroy()
        {
            _wictoryHandler.WindowsSwithed -= OnWindowsSwitched;
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }

        protected virtual void OnWindowsSwitched()
        {
            _reward = _rewardCounter.GetReward();
            _rewardValue.text = $"{_reward}";

            Show();
        }

        protected void LoadNextScene()
        {
            _loadingCurtain.Show();
            _gameStateMachine.Enter<MainMenuState>();
            SetNextLevel();
        }

        private void OnContinueButtonClicked()
        {
            if (_isContinueButtonClicked)
                return;

            _isContinueButtonClicked = true;

            StartCoroutine(Animator(_staticDataService.AnimationsConfig.WalletValueChangingDuration, callback: LoadNextScene));

            _walletPanel.CreditReward(_reward, _staticDataService.AnimationsConfig.WalletValueChangingDuration);
            _moneyEffect.Play();

            _persistentProgressService.Progress.Wallet.Give(_reward);
        }

        private void SetNextLevel()
        {
            uint currentLevelIndex = _persistentProgressService.Progress.CurrentLevelIndex;
            BiomeType currentBiomeType = _persistentProgressService.Progress.CurrentBiomeType;

            if (currentLevelIndex >= _staticDataService.GetLevelsSequence(currentBiomeType).Sequence.Length - 1)
            {
                int lenght = System.Enum.GetValues(typeof(BiomeType)).Length;
                int nextLevelType = (int)currentBiomeType + 1;

                nextLevelType = nextLevelType >= lenght ? 0 : nextLevelType;
                _persistentProgressService.Progress.CurrentBiomeType = (BiomeType)nextLevelType;
                _persistentProgressService.Progress.CurrentLevelIndex = 0;
            }
            else
            {
                _persistentProgressService.Progress.CurrentLevelIndex++;
            }

            _persistentProgressService.Progress.CompletedLevelsCount++;
            _saveLoadService.SaveProgress();
        }

        private IEnumerator Animator(float duration, Action callback)
        {
            float progress;
            float passedTime = 0;
            uint startValue = _reward;
            bool isAnimated = true;

            while (isAnimated)
            {
                progress = passedTime / duration;
                passedTime += Time.deltaTime;

                int value = (int)Mathf.Lerp(startValue, 0, progress);
                _rewardValue.text = value.ToString();

                if (value == 0)
                    isAnimated = false;

                yield return null;
            }

            yield return new WaitForSeconds(CloseDelay);

            callback?.Invoke();
        }
    }
}
