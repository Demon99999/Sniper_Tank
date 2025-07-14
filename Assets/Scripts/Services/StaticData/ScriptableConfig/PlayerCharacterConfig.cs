using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig
{
    [CreateAssetMenu(fileName = "PlayerCharacterConfig", menuName = "Configs/Create new player character config", order = 51)]
    public class PlayerCharacterConfig : ScriptableObject, IConfig<string>
    {
        public string Id;
        public bool IsUnlockedOnStart;
        public AssetReferenceGameObject AssetReference;
        public AssetReference Icon;

        public string Key => Id;
    }
}