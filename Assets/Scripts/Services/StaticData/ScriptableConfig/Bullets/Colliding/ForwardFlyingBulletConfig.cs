using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig.Bullets.Colliding
{
    [CreateAssetMenu(fileName = "ForwardFlyingBulletConfig", menuName = "Configs/Bullets/Create new forward flying bullet config", order = 51)]
    public class ForwardFlyingBulletConfig : CollidingBulletConfig, IConfig<ForwardFlyingBulletType>
    {
        public ForwardFlyingBulletType Type;

        public ForwardFlyingBulletType Key => Type;
    }
}