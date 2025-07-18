using Assets.Scripts.Enum;
using Assets.Scripts.GamePlay.Camera;
using Assets.Scripts.GamePlay.Player;
using Assets.Scripts.Infrastructure.Factoris.GamePlayFactory;
using Assets.Scripts.Infrastructure.Factoris.TankFactory;
using Assets.Scripts.Infrastructure.Factoris.UI;
using Assets.Scripts.Services.PersistentProgressServices;
using Assets.Scripts.Services.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class DroneLevelBootstrapper : GameplayBootstrapper
    {
        public DroneLevelBootstrapper(
            IUiFactory uiFactory,
            IGameplayFactory gameplayFactory,
            ITankFactory tankFactory,
            PlayerPoint playerPoint,
            AimingCameraPoint aimingCameraPoint,
            IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService,
            VictoryWindowType victoryWindowType)
            : base(uiFactory, gameplayFactory, tankFactory, playerPoint, aimingCameraPoint, staticDataService, persistentProgressService, victoryWindowType)
        {
        }

        protected override async UniTask<GameplayCamera> CreateCamera(IGameplayFactory gameplayFactory)
        {
            GameplayCamera gameplayCamera = await base.CreateCamera(gameplayFactory);
            await gameplayFactory.CreateCameraNoise(gameplayCamera.transform);

            return gameplayCamera;
        }

        protected override async UniTask CreateAimingVirtualCamera(IGameplayFactory gameplayFactory, Vector3 position,
            Quaternion rotation)
        {
            await gameplayFactory.CreateRotationVirtualCamera(position, rotation);
        }

        protected override async UniTask CreateDefeatWndow(IUiFactory uiFactory)
        {
            await uiFactory.CreateDroneDefeatWindow();
        }

        protected override async UniTask CreateGameplayWindow(IUiFactory uiFactory)
        {
            await uiFactory.CreateDroneGameplayWindow();
        }

        protected override async UniTask CreatePlayerWrapper(ITankFactory tankFactory, PlayerPoint playerPoint)
        {
            await tankFactory.CreatePlayerDroneWrapper(playerPoint.transform.position, playerPoint.transform.rotation);
        }
    }
}
