using Assets.Scripts.Services.AssetManagementServices;
using Assets.Scripts.Ui;
using Assets.Scripts.Ui.Game.Aim;
using Assets.Scripts.Ui.Game.BulletsPanel;
using Assets.Scripts.Ui.MainMenu.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Infrastructure.Factoris.UI
{
    public class UiFactoryInstaller : Installer<UiFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IUiFactory>()
                .To<UiFactory>()
                .AsSingle();

            Container
                .BindFactory<string, UniTask<Window>, Window.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<Window>>();

            Container
                .BindFactory<string, Transform, UniTask<SelectingPanelElement>, SelectingPanelElement.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<SelectingPanelElement>>();

            Container
                .BindFactory<string, Transform, UniTask<ProgressBarElement>, ProgressBarElement.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<ProgressBarElement>>();

            Container
                .BindFactory<string, Transform, UniTask<BulletIcon>, BulletIcon.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<BulletIcon>>();

            Container
                .BindFactory<string, Transform, UniTask<SuperBulletIcon>, SuperBulletIcon.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<SuperBulletIcon>>();
        }
    }
}
