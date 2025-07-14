using Assets.Scripts.Enum;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig.Level
{
    [CreateAssetMenu(fileName = "LevelsSequenceConfig", menuName = "Configs/Create new levels sequence config", order = 51)]
    public class LevelsSequenceConfig : ScriptableObject, IConfig<BiomeType>
    {
        public BiomeType Type;
        public string[] Sequence;
        public string MainMenuScene;
        public AssetReference IconReference;

        public BiomeType Key => Type;

        public string GetLevel(uint index)
        {
            if (index >= Sequence.Length)
            {
                Debug.LogError("Level index incorrect");
                return string.Empty;
            }

            return Sequence[index];
        }
    }
}