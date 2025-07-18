using Assets.Scripts.Enum;
using Assets.Scripts.GamePlay.Camera;
using Assets.Scripts.GamePlay.Handlers;
using Assets.Scripts.GamePlay.Player;
using Assets.Scripts.Infrastructure.Factoris.Bullets;
using Assets.Scripts.Infrastructure.Factoris.GamePlayFactory;
using Assets.Scripts.Infrastructure.Factoris.TankFactory;
using Assets.Scripts.Infrastructure.Factoris.UI;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay
{
    public abstract class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private PlayerPoint _playerPoint;
        [SerializeField] private AimingCameraPoint _aimingCameraPoint;
        [SerializeField] private VictoryWindowType _wictoryWindowType;

        public override void InstallBindings()
        {
            BindGameplayBootstrapper();
            BindUiFactory();
            BindGameplayFactory();
            BindBulletFactory();
            BindTankFactory();
            BindPlayerPoint();
            BindAimingCamera();
            BindAiming();
            BindDefeatHandler();
            BindWinHandler();
            BindRewardCounter();
            BindWictoryWindowType();
        }

        protected abstract void BindGameplayBootstrapper();

        protected abstract void BindAiming();

        private void BindWictoryWindowType()
        {
            Container.BindInstance(_wictoryWindowType).AsSingle();
        }

        private void BindRewardCounter()
        {
            Container.Bind<RewardCounter>().AsSingle();
        }

        private void BindDefeatHandler()
        {
            Container.Bind<DefeatHandler>().AsSingle();
        }

        private void BindWinHandler()
        {
            Container.BindInterfacesAndSelfTo<VictoryHandler>().AsSingle();
        }

        private void BindAimingCamera()
        {
            Container.BindInstance(_aimingCameraPoint);
        }

        private void BindPlayerPoint()
        {
            Container.BindInstance(_playerPoint).AsSingle();
        }

        private void BindTankFactory()
        {
            TankFactoryInstaller.Install(Container);
        }

        private void BindBulletFactory()
        {
            BulletFactoryInstaller.Install(Container);
        }

        private void BindGameplayFactory()
        {
            GameplayFactoryInstaller.Install(Container);
        }

        private void BindUiFactory()
        {
            UiFactoryInstaller.Install(Container);
        }
    }
}
