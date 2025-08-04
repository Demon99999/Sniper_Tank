using System;
using Assets.Scripts.Services.CoroutineRunnerServices;
using Assets.Scripts.Services.InputService;
using Assets.Scripts.Services.StaticData;
using YG;

namespace Assets.Scripts.GamePlay.Handlers
{
    public class DefeatHandler : GameplayHandler
    {
        private const int RewardID = 2;

        private IInputService _inputService;

        public DefeatHandler(ICoroutineRunner coroutineRunner, IStaticDataService staticDataService,
            IInputService inputService)
            : base(coroutineRunner, staticDataService)
        {
            _inputService = inputService;
            YandexGame.RewardVideoEvent += OnRewarded;
        }

        public event Action Defeated;
        public event Action WindowsSwitched;
        public event Action ProgressRecovered;

        public void OnDestroy()
        {
            YandexGame.RewardVideoEvent -= OnRewarded;
        }

        public void TryRecoveryProgress()
        {
            YandexGame.RewVideoShow(RewardID);
        }

        public void OnDefeat()
        {
            Defeated?.Invoke();
            _inputService.SetActive(false);
            StartTimer(callback: () => WindowsSwitched?.Invoke());
        }

        private void OnRewarded(int id)
        {
            if (id == RewardID)
            {
                _inputService.SetActive(true);
                ProgressRecovered?.Invoke();
            }
        }
    }
}