using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig.Bullets
{
    public class BulletConfig : ScriptableObject
    {
        public AssetReferenceGameObject AssetReference;
        public float ExplosionLifeTime;
        public uint ExplosionForce;
        public float ExplosionRadius;
        public uint Damage;
    }
}