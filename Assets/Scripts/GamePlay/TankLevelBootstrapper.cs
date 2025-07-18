using Assets.Scripts.Data;
using Assets.Scripts.Enum;
using Assets.Scripts.GamePlay.Camera;
using Assets.Scripts.GamePlay.Player;
using Assets.Scripts.GamePlay.Player.Wrappers;
using Assets.Scripts.GamePlay.Tanks;
using Assets.Scripts.Infrastructure.Factoris.GamePlayFactory;
using Assets.Scripts.Infrastructure.Factoris.TankFactory;
using Assets.Scripts.Infrastructure.Factoris.UI;
using Assets.Scripts.Services.PersistentProgressServices;
using Assets.Scripts.Services.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class TankLevelBootstrapper : GameplayBootstrapper
    {
        public TankLevelBootstrapper(
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

        protected override async UniTask CreateAimingVirtualCamera(IGameplayFactory gameplayFactory, Vector3 position,
            Quaternion rotation)
        {
            await gameplayFactory.CreateAimingVirtualCamera(position, rotation);
        }

        protected async override UniTask CreateDefeatWndow(IUiFactory uiFactory)
        {
            await uiFactory.CreateTankDefeatWindow();
        }

        protected override async UniTask CreateGameplayWindow(IUiFactory uiFactory)
        {
            await uiFactory.CreateTankGameplayWindow();
        }

        protected override async UniTask CreatePlayerWrapper(ITankFactory tankFactory, PlayerPoint playerPoint)
        {
            TankData tankData = PersistentProgressService.Progress.GetSelectedTank();

            PlayerTankWrapper playerTankWrapper = await tankFactory.CreatePlayerTankWrapper(
                tankData.Level,
                playerPoint.transform.position,
                playerPoint.transform.rotation);

            Tank tank = await tankFactory.CreateTank(
                tankData.Level,
                playerTankWrapper.transform.position,
                playerTankWrapper.transform.rotation,
                playerTankWrapper.transform,
                tankData.SkinId,
                tankData.DecalId,
                false);

            playerTankWrapper.Initialize(tank.BulletPoints, tank.Turret);
        }
    }
}
