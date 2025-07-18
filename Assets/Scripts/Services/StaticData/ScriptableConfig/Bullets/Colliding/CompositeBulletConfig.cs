using UnityEngine;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig.Bullets.Colliding
{
    [CreateAssetMenu(fileName = "CompositeBulletConfig", menuName = "Configs/Bullets/Create new composite bullet config", order = 51)]

    public class CompositeBulletConfig : CollidingBulletConfig
    {
        public uint BombsCount;
    }
}