using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Ui.Game.VictoryWindow
{
    public class RouletteVictoryWindow : VictoryWindow
    {
        [SerializeField] private Roulette _roulette;
        [SerializeField] private TMP_Text _continueButtonRewardValue;
        [SerializeField] private CanvasGroup _continueButtonCanvasGroup;
        [SerializeField] private float _continueButtonShowDuration;

        protected override void Start()
        {
            base.Start();
            _roulette.Rewarded += OnRouletteRewarded;
            SetContinueButtonActive(false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _roulette.Rewarded -= OnRouletteRewarded;
        }

        protected override void OnWindowsSwitched()
        {
            uint reward = RewardCounter.GetReward();
            _continueButtonRewardValue.text = reward.ToString();
            _roulette.Work(reward);

            StartCoroutine(ContinueButtonShower());

            base.OnWindowsSwitched();
        }

        private void OnRouletteRewarded(uint reward)
        {
            PersistentProgressService.Progress.Wallet.Give(reward);
            LoadNextScene();
        }

        private void SetContinueButtonActive(bool isActive)
        {
            _continueButtonCanvasGroup.alpha = isActive ? 1 : 0;
            _continueButtonCanvasGroup.interactable = isActive;
            _continueButtonCanvasGroup.blocksRaycasts = isActive;
        }

        private IEnumerator ContinueButtonShower()
        {
            yield return new WaitForSeconds(_continueButtonShowDuration);

            SetContinueButtonActive(true);
        }
    }
}
