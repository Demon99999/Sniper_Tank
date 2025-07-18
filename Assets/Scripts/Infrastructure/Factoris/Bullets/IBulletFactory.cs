using Assets.Scripts.Enum;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factoris.Bullets
{
    public interface IBulletFactory
    {
        UniTask CreateForwardFlyingBullet(ForwardFlyingBulletType type, Vector3 position, Quaternion rotation);
        UniTask CreateHomingBullet(HomingBulletType type, Vector3 position, Quaternion rotation);
        UniTask CreateMuzzle(MuzzleType type, Vector3 position, Quaternion rotation);
        UniTask CreateTargetingLaser(Vector3 position, Vector3 targetPosition);
        UniTask CreateTransmittingLaser(Vector3 positoin, Quaternion rotation);
        UniTask CreateCompositeBullet(Vector3 position, Quaternion rotation);
    }
}