using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enum;
using Assets.Scripts.Services.AssetManagementServices;
using Assets.Scripts.Services.StaticData.ScriptableConfig;
using Assets.Scripts.Services.StaticData.ScriptableConfig.Bullets.Colliding;
using Assets.Scripts.Services.StaticData.ScriptableConfig.Bullets.Laser;
using Assets.Scripts.Services.StaticData.ScriptableConfig.Level;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetsProvider;

        private Dictionary<string, LevelConfig> _levelConfigs;
        private Dictionary<EnemyType, EnemyConfig> _enemyConfigs;
        private Dictionary<ForwardFlyingBulletType, ForwardFlyingBulletConfig> _forwardFlyingBulletConfigs;
        private Dictionary<HomingBulletType, HomingBulletConfig> _homingBulletConfigs;
        private Dictionary<uint, TankConfig> _tankConfigs;
        private Dictionary<string, TankSkinConfig> _tankSkinConfigs;
        private Dictionary<string, DecalConfig> _decalConfigs;
        private Dictionary<MuzzleType, MuzzleConfig> _muzzleConfigs;
        private Dictionary<BiomeType, LevelsSequenceConfig> _levelsSequenceConfigs;
        private Dictionary<string, PlayerCharacterConfig> _playerCharacterConfigs;

        public StaticDataService(IAssetProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;
        }

        public LaserConfig DiretionalLaserConfig { get; private set; }
        public TargetingLaserConfig TargetingLaserConfig { get; private set; }
        public TransmittingLaserConfig TransmittedLaserConfig { get; private set; }
        public CompositeBulletConfig CompositeBulletConfig { get; private set; }
        public TankConfig[] TankConfigs => _tankConfigs.Values.ToArray();
        public TankSkinConfig[] TankSkinConfigs => _tankSkinConfigs.Values.ToArray();
        public DecalConfig[] DecalConfigs => _decalConfigs.Values.ToArray();
        public AnimationsConfig AnimationsConfig { get; private set; }
        public AimingConfig AimingConfig { get; private set; }
        public DestructionConfig DestructionConfig { get; private set; }
        public GameplaySettingsConfig GameplaySettingsConfig { get; private set; }
        public EnviromentExplosionsConfig EnviromentExplosionsConfig { get; private set; }
        public PlayerCharacterConfig[] PlayerCharacterCofigs => _playerCharacterConfigs.Values.ToArray();
        public MainMenuSettingsConfig MainMenuSettingsConfig { get; private set; }

        public async UniTask InitializeAsync()
        {
            AnimationsConfig = await LoadConfig<AnimationsConfig>();
            AimingConfig = await LoadConfig<AimingConfig>();
            DestructionConfig = await LoadConfig<DestructionConfig>();
            GameplaySettingsConfig = await LoadConfig<GameplaySettingsConfig>();
            EnviromentExplosionsConfig = await LoadConfig<EnviromentExplosionsConfig>();
            MainMenuSettingsConfig = await LoadConfig<MainMenuSettingsConfig>();
            DiretionalLaserConfig = await LoadConfig<LaserConfig>();
            TargetingLaserConfig = await LoadConfig<TargetingLaserConfig>();
            TransmittedLaserConfig = await LoadConfig<TransmittingLaserConfig>();
            CompositeBulletConfig = await LoadConfig<CompositeBulletConfig>();
            _forwardFlyingBulletConfigs = await LoadConfigs<ForwardFlyingBulletType, ForwardFlyingBulletConfig>();
            _homingBulletConfigs = await LoadConfigs<HomingBulletType, HomingBulletConfig>();
            _levelConfigs = await LoadConfigs<string, LevelConfig>();
            _enemyConfigs = await LoadConfigs<EnemyType, EnemyConfig>();
            _tankConfigs = await LoadConfigs<uint, TankConfig>();
            _tankSkinConfigs = await LoadConfigs<string, TankSkinConfig>();
            _decalConfigs = await LoadConfigs<string, DecalConfig>();
            _muzzleConfigs = await LoadConfigs<MuzzleType, MuzzleConfig>();
            _levelsSequenceConfigs = await LoadConfigs<BiomeType, LevelsSequenceConfig>();
            _playerCharacterConfigs = await LoadConfigs<string, PlayerCharacterConfig>();

        }

        public LevelsSequenceConfig GetLevelsSequence(BiomeType type)
        {
            return _levelsSequenceConfigs.TryGetValue(type, out LevelsSequenceConfig config) ? config : null;
        }

        public PlayerCharacterConfig GetPlayerCharacter(string id)
        {
            return _playerCharacterConfigs.TryGetValue(id, out PlayerCharacterConfig config) ? config : null;
        }

        public MuzzleConfig GetMuzzle(MuzzleType type)
        {
            return _muzzleConfigs.TryGetValue(type, out MuzzleConfig config) ? config : null;
        }

        public DecalConfig GetDecal(string id)
        {
            return _decalConfigs.TryGetValue(id, out DecalConfig config) ? config : null;
        }

        public TankSkinConfig GetSkin(string id)
        {
            return _tankSkinConfigs.TryGetValue(id, out TankSkinConfig config) ? config : null;
        }

        public TankConfig GetTank(uint level)
        {
            return _tankConfigs.TryGetValue(level, out TankConfig config) ? config : null;
        }

        public HomingBulletConfig GetBullet(HomingBulletType type)
        {
            return _homingBulletConfigs.TryGetValue(type, out HomingBulletConfig config) ? config : null;
        }

        public ForwardFlyingBulletConfig GetBullet(ForwardFlyingBulletType type)
        {
            return _forwardFlyingBulletConfigs.TryGetValue(type, out ForwardFlyingBulletConfig config) ? config : null;
        }

        public EnemyConfig GetEnemy(EnemyType type)
        {
            return _enemyConfigs.TryGetValue(type, out EnemyConfig config) ? config : null;
        }

        public LevelConfig GetLevel(string key)
        {
            return _levelConfigs.TryGetValue(key, out LevelConfig config) ? config : null;
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
