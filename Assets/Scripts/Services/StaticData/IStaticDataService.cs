using Assets.Scripts.Enum;
using Assets.Scripts.Services.StaticData.ScriptableConfig.Level;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Services.StaticData
{
    public interface IStaticDataService
    {
        UniTask InitializeAsync();

        LevelsSequenceConfig GetLevelsSequence(BiomeType type);
    }
}
