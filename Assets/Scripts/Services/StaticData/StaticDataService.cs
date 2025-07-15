using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enum;
using Assets.Scripts.Services.AssetManagementServices;
using Assets.Scripts.Services.StaticData.ScriptableConfig.Level;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetsProvider;

        private Dictionary<BiomeType, LevelsSequenceConfig> _levelsSequenceConfigs;

        public StaticDataService(IAssetProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;
        }

        public async UniTask InitializeAsync()
        {
            //UniTask.Create(async () => _levelsSequenceConfigs = await LoadConfigs<BiomeType, LevelsSequenceConfig>());

            _levelsSequenceConfigs = await LoadConfigs<BiomeType, LevelsSequenceConfig>();
        }

        public LevelsSequenceConfig GetLevelsSequence(BiomeType type)
        {
            return _levelsSequenceConfigs.TryGetValue(type, out LevelsSequenceConfig config) ? config : null;
        }



        private async UniTask<TConfig> LoadConfig<TConfig>()
            where TConfig : class
        {
            TConfig[] configs = await GetConfigs<TConfig>();

            return configs.FirstOrDefault();
        }

        private async UniTask<Dictionary<TKey, TConfig>> LoadConfigs<TKey, TConfig>()
            where TConfig : class, IConfig<TKey>
        {
            TConfig[] configs = await GetConfigs<TConfig>();

            return configs.ToDictionary(value => value.Key, value => value);
        }

        private async UniTask<TConfig[]> GetConfigs<TConfig>()
            where TConfig : class
        {
            List<string> keys = await GetConfigKeys<TConfig>();
            return await _assetsProvider.LoadAll<TConfig>(keys);
        }

        private async UniTask<List<string>> GetConfigKeys<TConfig>() =>
            await _assetsProvider.GetAssetsListByLabel<TConfig>(AssetLabels.Configs);
    }
}
