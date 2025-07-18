using Assets.Scripts.MenuScene.Desk;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.Infrastructure.Factoris.MenuMain
{
    public class MainMenuFactory : IMainMenuFactory
    {
        private readonly DiContainer _container;
        private readonly Desk.Factory _deskFactory;

        public MainMenuFactory(DiContainer container, Desk.Factory deskFactory)
        {
            _container = container;
            _deskFactory = deskFactory;
        }

        public async UniTask CreateDesk()
        {
            Desk desk = await _deskFactory.Create(MainMenuFactoryAssets.Desk);

            _container.BindInstance(desk).AsSingle();
        }
    }
}
