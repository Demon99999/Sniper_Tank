using Assets.Scripts.GamePlay.Camera;
using Assets.Scripts.GamePlay.Enemis;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factoris.GamePlayFactory
{
    public interface IGameplayFactory
    {
        UniTask CreateAimingVirtualCamera(Vector3 position, Quaternion rotation);
        UniTask<GameplayCamera> CreateCamera();
        UniTask CreateCameraNoise(Transform parent);
        UniTask<Enemy> CreateEnemy(EnemyType type, Vector3 position, Quaternion rotation);
        UniTask CreateRotationVirtualCamera(Vector3 position, Quaternion rotation);
        UniTask<UiCamera> CreateUiCamra();
    }
}