using Assets.Scripts.GamePlay.Camera;
using Assets.Scripts.GamePlay.Enemis;
using Assets.Scripts.GamePlay.Handlers;
using Assets.Scripts.GamePlay.Player;
using Assets.Scripts.Services.StaticData;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Infrastructure.Factoris.GamePlayFactory
{
    public class GameplayFactory : IGameplayFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly DiContainer _container;
        private readonly GameplayCamera.Factory _gameplayCameraFactory;
        private readonly Enemy.Factory _enemyFactory;
        private readonly RotationCamera.Factory _rotationCameraFactory;
        private readonly VictoryHandler _winHandler;
        private readonly CameraNoise.Factory _cameraNoiseFactory;
        private readonly UiCamera.Factory _uiCameraFactory;

        public GameplayFactory(
            IStaticDataService staticDataService,
            DiContainer container,
            GameplayCamera.Factory gameplayCameraFactory,
            Enemy.Factory enemyFactory,
            RotationCamera.Factory aimingFactory,
            VictoryHandler winHandler,
            CameraNoise.Factory cameraNoiseFactory,
            UiCamera.Factory uiCameraFactory)
        {
            _staticDataService = staticDataService;
            _container = container;
            _gameplayCameraFactory = gameplayCameraFactory;
            _enemyFactory = enemyFactory;
            _rotationCameraFactory = aimingFactory;
            _winHandler = winHandler;
            _cameraNoiseFactory = cameraNoiseFactory;
            _uiCameraFactory = uiCameraFactory;
        }

        public async UniTask<UiCamera> CreateUiCamra()
        {
            UiCamera uiCamera = await _uiCameraFactory.Create(GameplayFactoryAssets.UiCamera);
            _container.BindInstance(uiCamera).AsSingle();

            return uiCamera;
        }

        public async UniTask CreateCameraNoise(Transform parent)
        {
            CameraNoise cameraNoise = await _cameraNoiseFactory.Create(GameplayFactoryAssets.CameraNoise, parent);
            _container.BindInstance(cameraNoise).AsSingle();
        }

        public async UniTask CreateRotationVirtualCamera(Vector3 position, Quaternion rotation)
        {
            RotationCamera rotationCamera = await _rotationCameraFactory.Create(GameplayFactoryAssets.RotationCamera, position, rotation);
            _container.BindInstance(rotationCamera).AsSingle();
        }

        public async UniTask CreateAimingVirtualCamera(Vector3 position, Quaternion rotation)
        {
            RotationCamera rotationCamera = await _rotationCameraFactory.Create(GameplayFactoryAssets.AimingCamera, position, rotation);
            _container.BindInstance(rotationCamera.GetComponentInChildren<CameraShaking>()).AsSingle();
        }

        public async UniTask<GameplayCamera> CreateCamera()
        {
            GameplayCamera gameplayCamera = await _gameplayCameraFactory.Create(GameplayFactoryAssets.Camera);

            _container.BindInstance(gameplayCamera).AsSingle();

            return gameplayCamera;
        }

        public async UniTask<Enemy> CreateEnemy(EnemyType type, Vector3 position, Quaternion rotation)
        {
            Enemy enemy = await _enemyFactory.Create(_staticDataService.GetEnemy(type).AssetReference, position, rotation);

            _winHandler.AddEnemy(enemy);

            return enemy;
        }
    }
}
