using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig
{
    [CreateAssetMenu(fileName = "TankConfig", menuName = "Configs/Create new tank config", order = 51)]
    public class TankConfig : ScriptableObject, IConfig<uint>
    {
        public uint Level;
        public AssetReferenceGameObject AssetReference;
        public bool IsUnlockOnStart;
        public AssetReference BaseMaterialAssetReference;
        public AssetReferenceGameObject MainMenuWrapperAssetReference;
        public AssetReferenceGameObject GameplayWrapperAssetReference;
        public string Name;
        public AssetReference Icon;

        public uint Key => Level;
    }
}