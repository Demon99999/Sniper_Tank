using Assets.Scripts.Enum;
using Assets.Scripts.Ui;
using Assets.Scripts.Ui.Game;
using Assets.Scripts.Ui.Game.Aim;
using Assets.Scripts.Ui.Game.BulletsPanel;
using Assets.Scripts.Ui.Game.Gameplay;
using Assets.Scripts.Ui.MainMenu;
using Assets.Scripts.Ui.MainMenu.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Infrastructure.Factoris.UI
{
    public class UiFactory : IUiFactory
    {
        private readonly Window.Factory _windowFactory;
        private readonly DiContainer _container;
        private readonly SelectingPanelElement.Factory _selectingPanelElementFactory;
        private readonly ProgressBarElement.Factory _progressBarElementFactory;
        private readonly BulletIcon.Factory _bulletIconFactory;
        private readonly SuperBulletIcon.Factory _superBulletIconFactory;

        public UiFactory(
            Window.Factory windowFactory,
            DiContainer container,
            SelectingPanelElement.Factory selectingPanelElementFactory,
            ProgressBarElement.Factory progressBarElementFactory,
            BulletIcon.Factory bulletIconFactory,
            SuperBulletIcon.Factory superBulletIconFactory)
        {
            _windowFactory = windowFactory;
            _container = container;
            _selectingPanelElementFactory = selectingPanelElementFactory;
            _progressBarElementFactory = progressBarElementFactory;
            _bulletIconFactory = bulletIconFactory;
            _superBulletIconFactory = superBulletIconFactory;
        }

        public UniTask<SelectingPanelElement> CreateDecalPanel(Transform parent) =>
            _selectingPanelElementFactory.Create(UiFactoryAssets.DecalPanel, parent);

        public async UniTask CreateRestartWindow()
        {
            Window window = await _windowFactory.Create(UiFactoryAssets.RestartWindow);
            _container.BindInstance(window as RestartWindow).AsSingle();
        }

        public async UniTask<BulletIcon> CreateDroneBulletIcon(Transform parent) =>
            await _bulletIconFactory.Create(UiFactoryAssets.DroneBulletIcon, parent);

        public async UniTask<SuperBulletIcon> CreateSuperBulletIcon(Transform parent) =>
            await _superBulletIconFactory.Create(UiFactoryAssets.SuperBulletIcon, parent);

        public async UniTask<BulletIcon> CreateTankBulletIcon(Transform parent) =>
            await _bulletIconFactory.Create(UiFactoryAssets.TankBulletIcon, parent);

        public async UniTask<ProgressBarElement> CreateProgressBarElement(Transform parent) =>
            await _progressBarElementFactory.Create(UiFactoryAssets.ProgressBarElement, parent);

        public async UniTask CreateOptionsWindow()
        {
            Window window = await _windowFactory.Create(UiFactoryAssets.OptionsWindow);
            _container.BindInstance(window as OptionsWindow).AsSingle();
        }

        public async UniTask<SelectingPanelElement> CreateCharacterSkinPanel(Transform parent) =>
            await _selectingPanelElementFactory.Create(UiFactoryAssets.CharacterSkinPanel, parent);

        public async UniTask<GameplayLoadingCurtain> CreateLoadingCurtain()
        {
            GameplayLoadingCurtain loadingCurtain = await _windowFactory.Create(UiFactoryAssets.LoadingCurtain) as GameplayLoadingCurtain;
            _container.BindInstance(loadingCurtain).AsSingle();

            return loadingCurtain;
        }

        public async UniTask CreateWictroyWindow(VictoryWindowType type)
        {
            switch (type)
            {
                case VictoryWindowType.Default:
                    await _windowFactory.Create(UiFactoryAssets.VictoryWindow);
                    break;
                case VictoryWindowType.Roulette:
                    await _windowFactory.Create(UiFactoryAssets.RouletteVictoryWindow);
                    break;
                case VictoryWindowType.CharacterReward:
                    await _windowFactory.Create(UiFactoryAssets.CharacterRewardVictoryWindow);
                    break;
            }
        }

        public async UniTask CreateTankDefeatWindow()
        {
            await _windowFactory.Create(UiFactoryAssets.TankDefeatWindow);
        }

        public async UniTask CreateDroneDefeatWindow()
        {
            await _windowFactory.Create(UiFactoryAssets.DroneDefeatWindow);
        }

        public async UniTask<SelectingPanelElement> CreateUnlockingPanel(Transform parent)
        {
            return await _selectingPanelElementFactory.Create(UiFactoryAssets.UnlockingPanel, parent);
        }

        public async UniTask<SelectingPanelElement> CreateTankPanel(Transform parent)
        {
            return await _selectingPanelElementFactory.Create(UiFactoryAssets.TankPanel, parent);
        }

        public async UniTask CreateTankGameplayWindow()
        {
            await CreateGameplayWindow(UiFactoryAssets.TankGameplayWindow);
        }

        public async UniTask CreateDroneGameplayWindow()
        {
            await CreateGameplayWindow(UiFactoryAssets.DroneGameplayWindow);
        }

        public async UniTask<MainMenuWindow> CreateMainMenu()
        {
            MainMenuWindow mainMenuWindow = await _windowFactory.Create(UiFactoryAssets.MainMenuWindow) as MainMenuWindow;

            _container.BindInstance(mainMenuWindow).AsSingle();

            return mainMenuWindow;
        }

        private async UniTask CreateGameplayWindow(string assetPath)
        {
            GameplayWindow gameplayWindow = await _windowFactory.Create(assetPath) as GameplayWindow;

            _container.BindInstance(gameplayWindow).AsSingle();
        }
    }
}
