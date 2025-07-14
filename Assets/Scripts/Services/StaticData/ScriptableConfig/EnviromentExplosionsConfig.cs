using UnityEngine;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig
{
    [CreateAssetMenu(fileName = "EnviromentExplosionsConfig", menuName = "Configs/Create new enviroment explosions config", order = 51)]
    public class EnviromentExplosionsConfig : ScriptableObject
    {
        public float ExplosionRadius;
        public uint Damage;
        public uint ExplosionForce;
    }
}