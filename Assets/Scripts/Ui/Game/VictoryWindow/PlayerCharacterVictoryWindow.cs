using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Ui.Game.VictoryWindow
{
    public class PlayerCharacterVictoryWindow : VictoryWindow
    {
        [SerializeField] private PlayerCharacterRewardPanel _playerCharacterRewardPanel;
        [SerializeField] private CanvasGroup _continueButtonCanvasGroup;
        [SerializeField] private float _continueButtonShowDuration;

        [Inject]
        private void Construct()
        {
            SetContinueButtonActive(false);
        }

        protected override async void OnWindowsSwitched()
        {
            await _playerCharacterRewardPanel.GenerateCharacter();
            base.OnWindowsSwitched();

            StartCoroutine(ContinueButtonShower());
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
