using Assets.Scripts.GamePlay.Camera;
using Assets.Scripts.GamePlay.Enemis;
using Assets.Scripts.GamePlay.Player;
using Assets.Scripts.Services.AssetManagementServices;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Scripts.Infrastructure.Factoris.GamePlayFactory
{
    public class GameplayFactoryInstaller : Installer<GameplayFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container
               .Bind<IGameplayFactory>()
               .To<GameplayFactory>()
               .AsSingle();

            Container
                .BindFactory<string, UniTask<GameplayCamera>, GameplayCamera.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<GameplayCamera>>();

            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Enemy>, Enemy.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<Enemy>>();

            Container
                .BindFactory<string, Vector3, Quaternion, UniTask<RotationCamera>, RotationCamera.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<RotationCamera>>();

            Container
                .BindFactory<string, Transform, UniTask<CameraNoise>, CameraNoise.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<CameraNoise>>();

            Container
                .BindFactory<string, UniTask<UiCamera>, UiCamera.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<UiCamera>>();
        }
    }
}
