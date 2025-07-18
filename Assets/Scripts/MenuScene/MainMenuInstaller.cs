using Assets.Scripts.Infrastructure.Factoris.Bullets;
using Assets.Scripts.Infrastructure.Factoris.MenuMain;
using Assets.Scripts.Infrastructure.Factoris.TankFactory;
using Assets.Scripts.Infrastructure.Factoris.UI;
using Assets.Scripts.MenuScene.Desk;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.MenuScene
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuCamera _camera;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TankShootingHandler>().AsSingle();
            Container.Bind<TankBuyer>().AsSingle();
            TankFactoryInstaller.Install(Container);
            BulletFactoryInstaller.Install(Container);
            Container.BindInstance(_camera).AsSingle();
            Container.BindInterfacesAndSelfTo<DeskHandler>().AsSingle();
            MainMenuFactoryInstaller.Install(Container);
            UiFactoryInstaller.Install(Container);
            Container.BindInterfacesTo<MainMenuBootstrapper>().AsSingle().NonLazy();
        }
    }
}
