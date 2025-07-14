using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig
{
    [CreateAssetMenu(fileName = "DecalConfig", menuName = "Configs/Create new decal config", order = 51)]
    public class DecalConfig : ScriptableObject, IConfig<string>
    {
        public string Id;
        public AssetReference MaterialAssetReference;
        public bool IsUnlockedOnStart;
        public AssetReference SpriteAssetReference;
        public int SerialNumber;

        public string Key => Id;
    }
}