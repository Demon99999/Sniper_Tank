using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig.Bullets.Colliding
{
    [CreateAssetMenu(fileName = "HomingBulletConfig", menuName = "Configs/Bullets/Create new homing bullet config", order = 51)]
    public class HomingBulletConfig : CollidingBulletConfig, IConfig<HomingBulletType>
    {
        public HomingBulletType Type;
        public uint SearchRadius;
        public uint RotationSpeed;
        public float TargetingDelay;

        public HomingBulletType Key => Type;
    }
}