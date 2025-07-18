using Assets.Scripts.Enum;
using Assets.Scripts.Ui;
using Assets.Scripts.Ui.Game;
using Assets.Scripts.Ui.Game.Aim;
using Assets.Scripts.Ui.Game.BulletsPanel;
using Assets.Scripts.Ui.MainMenu;
using Assets.Scripts.Ui.MainMenu.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factoris.UI
{
    public interface IUiFactory
    {
        UniTask<MainMenuWindow> CreateMainMenu();
        UniTask<SelectingPanelElement> CreateTankPanel(Transform parent);
        UniTask<SelectingPanelElement> CreateUnlockingPanel(Transform parent);
        UniTask CreateTankGameplayWindow();
        UniTask CreateTankDefeatWindow();
        UniTask CreateWictroyWindow(VictoryWindowType type);
        UniTask<GameplayLoadingCurtain> CreateLoadingCurtain();
        UniTask CreateDroneGameplayWindow();
        UniTask CreateDroneDefeatWindow();
        UniTask<SelectingPanelElement> CreateCharacterSkinPanel(Transform parent);
        UniTask CreateOptionsWindow();
        UniTask<ProgressBarElement> CreateProgressBarElement(Transform parent);
        UniTask<BulletIcon> CreateTankBulletIcon(Transform parent);
        UniTask<SuperBulletIcon> CreateSuperBulletIcon(Transform parent);
        UniTask<BulletIcon> CreateDroneBulletIcon(Transform parent);
        UniTask CreateRestartWindow();
        UniTask<SelectingPanelElement> CreateDecalPanel(Transform parent);
    }
}