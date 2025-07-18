using System.Collections.Generic;
using Assets.Scripts.Enum;
using Assets.Scripts.GamePlay.Camera;
using Assets.Scripts.GamePlay.Player;
using Assets.Scripts.Infrastructure.Factoris.GamePlayFactory;
using Assets.Scripts.Infrastructure.Factoris.TankFactory;
using Assets.Scripts.Infrastructure.Factoris.UI;
using Assets.Scripts.Services.PersistentProgressServices;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Services.StaticData.ScriptableConfig.Level;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay
{
    public abstract class GameplayBootstrapper : IInitializable
    {
        private readonly IUiFactory _uiFactory;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly ITankFactory _tankFactory;
        private readonly AimingCameraPoint _aimingCameraPoint;
        private readonly IStaticDataService _staticDataService;
        private readonly PlayerPoint _playerPoint;
        private readonly VictoryWindowType _victoryWindowType;

        protected readonly IPersistentProgressService PersistentProgressService;

        public GameplayBootstrapper(
            IUiFactory uiFactory,
            IGameplayFactory gameplayFactory,
            ITankFactory tankFactory,
            PlayerPoint playerPoint,
            AimingCameraPoint aimingCameraPoint,
            IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService,
            VictoryWindowType victoryWindowType)
        {
            _uiFactory = uiFactory;
            _gameplayFactory = gameplayFactory;
            _tankFactory = tankFactory;
            _playerPoint = playerPoint;
            _aimingCameraPoint = aimingCameraPoint;
            _staticDataService = staticDataService;
            PersistentProgressService = persistentProgressService;
            _victoryWindowType = victoryWindowType;
        }

        public async void Initialize()
        {
            uint levelIndex = PersistentProgressService.Progress.CurrentLevelIndex;
            BiomeType biomeType = PersistentProgressService.Progress.CurrentBiomeType;
            LevelConfig levelConfig = _staticDataService.GetLevel(_staticDataService.GetLevelsSequence(biomeType).GetLevel(levelIndex));

            await CreateCamera(_gameplayFactory);
            await CreateAimingVirtualCamera(_gameplayFactory, _aimingCameraPoint.transform.position, _aimingCameraPoint.transform.rotation);

            await CreatePlayerWrapper(_tankFactory, _playerPoint);
            await CreateEnemies(levelConfig);
            await _uiFactory.CreateRestartWindow();
            await _uiFactory.CreateOptionsWindow();
            await CreateGameplayWindow(_uiFactory);
            await CreateDefeatWndow(_uiFactory);
            await _uiFactory.CreateLoadingCurtain();
            await _uiFactory.CreateWictroyWindow(_victoryWindowType);
        }

        protected virtual async UniTask<GameplayCamera> CreateCamera(IGameplayFactory gameplayFactory) =>
            await gameplayFactory.CreateCamera();

        protected abstract UniTask CreateDefeatWndow(IUiFactory uiFactory);

        protected abstract UniTask CreateAimingVirtualCamera(IGameplayFactory gameplayFactory, Vector3 position, Quaternion rotation);

        protected abstract UniTask CreatePlayerWrapper(ITankFactory tankFactory, PlayerPoint playerPoint);

        protected abstract UniTask CreateGameplayWindow(IUiFactory uiFactory);

        private async UniTask CreateEnemies(LevelConfig levelConfig)
        {
            List<UniTask> tasks = new List<UniTask>();

            foreach (HelicopterPointConfig helicopterPointConfig in levelConfig.HelicopterPoints)
                tasks.Add(helicopterPointConfig.Create(_gameplayFactory));

            foreach (EnemyMovementEngineryPointConfig enemyCarPointConfig in levelConfig.MovementEngineryPoints)
                tasks.Add(enemyCarPointConfig.Create(_gameplayFactory));

            foreach (PatrolingEnemyPointConfig patrolingEnemyPointConfig in levelConfig.PatrolingEnemyPoints)
                tasks.Add(patrolingEnemyPointConfig.Create(_gameplayFactory));

            foreach (StaticEnemyPointConfig staticEnemyPointConfig in levelConfig.StaticEnemyPoints)
                tasks.Add(staticEnemyPointConfig.Create(_gameplayFactory));

            await UniTask.WhenAll(tasks);
        }
    }
}
