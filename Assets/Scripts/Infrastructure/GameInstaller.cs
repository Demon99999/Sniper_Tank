using Assets.Scripts.Services.AssetManagementServices;
using Assets.Scripts.Services.CoroutineRunnerServices;
using Assets.Scripts.Services.InputService;
using Assets.Scripts.Services.PersistentProgressServices;
using Assets.Scripts.Services.SaveLoadProgressServices;
using Assets.Scripts.Services.SceneManagmentServices;
using Assets.Scripts.Services.StateMachine;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Ui.LoadingCurtain;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.Infrastructure
{
    public class GameInstaller : MonoInstaller, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            Bind();
        }

        private void Bind()
        {
            Container.BindInterfacesTo<InputService>().AsSingle();
            Container.Bind<StatesFactory>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
            Container.Bind(typeof(ICoroutineRunner)).FromInstance(this).AsSingle();
            Container.BindInterfacesTo<StaticDataService>().AsSingle();
            Container.BindInterfacesTo<PersistentProgressService>().AsSingle();
            Container.BindInterfacesTo<SaveLoadService>().AsSingle();

            Container
                .BindFactory<string, UniTask<LoadingCurtain>, LoadingCurtain.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<LoadingCurtain>>();

            Container
                .BindInterfacesAndSelfTo<LoadingCurtainProxy>()
                .AsSingle();
        }
    }
}