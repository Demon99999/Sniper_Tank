using Assets.Scripts.MenuScene.Desk;
using Assets.Scripts.Services.AssetManagementServices;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.Infrastructure.Factoris.MenuMain
{
    public class MainMenuFactoryInstaller : Installer<MainMenuFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainMenuFactory>().AsSingle();

            Container
                .BindFactory<string, UniTask<Desk>, Desk.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<Desk>>();
        }
    }
}
