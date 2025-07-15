using Assets.Scripts.Services.AssetManagementServices;
using Assets.Scripts.Services.CoroutineRunnerServices;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Ui.LoadingCurtain;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Services.StateMachine.States
{
    public class BootstapState : IState
    {
        private readonly IAssetProvider _assetProvider;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly LoadingCurtainProxy _loadingCurtainProyxy;

        public BootstapState(
            IAssetProvider assetProvider,
            GameStateMachine gameStateMachine,
            IStaticDataService staticDataService,
            ICoroutineRunner coroutineRunner,
            LoadingCurtainProxy loadingCurtainProyxy)
        {
            _assetProvider = assetProvider;
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _coroutineRunner = coroutineRunner;
            _loadingCurtainProyxy = loadingCurtainProyxy;
        }

        public async UniTask Enter()
        {
            await Initialize();

            _gameStateMachine.Enter<LoadProgressState>();
        }

        private async UniTask Initialize()
        {
            await _assetProvider.InitializeAsync();
            await _staticDataService.InitializeAsync();
            await _loadingCurtainProyxy.InitializeAsync();
        }

        public UniTask Exit()
        {
            _loadingCurtainProyxy.Show();
            return default;
        }
    }
}