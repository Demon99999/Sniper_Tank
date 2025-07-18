using Assets.Scripts.GamePlay.Player.Aim;

namespace Assets.Scripts.GamePlay
{
    public class TankLevelInstaller : GameplayInstaller
    {
        protected override void BindAiming()
        {
            Container.BindInterfacesAndSelfTo<TankAiming>().AsSingle();
        }

        protected override void BindGameplayBootstrapper()
        {
            Container.BindInterfacesTo<TankLevelBootstrapper>().AsSingle().NonLazy();
        }
    }
}
