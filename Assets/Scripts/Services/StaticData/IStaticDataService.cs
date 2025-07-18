using Assets.Scripts.Enum;
using Assets.Scripts.Services.StaticData.ScriptableConfig;
using Assets.Scripts.Services.StaticData.ScriptableConfig.Bullets.Colliding;
using Assets.Scripts.Services.StaticData.ScriptableConfig.Bullets.Laser;
using Assets.Scripts.Services.StaticData.ScriptableConfig.Level;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Services.StaticData
{
    public interface IStaticDataService
    {
        LaserConfig DiretionalLaserConfig { get; }
        TargetingLaserConfig TargetingLaserConfig { get; }
        TransmittingLaserConfig TransmittedLaserConfig { get; }
        CompositeBulletConfig CompositeBulletConfig { get; }
        TankConfig[] TankConfigs { get; }
        TankSkinConfig[] TankSkinConfigs { get; }
        DecalConfig[] DecalConfigs { get; }
        AnimationsConfig AnimationsConfig { get; }
        AimingConfig AimingConfig { get; }
        DestructionConfig DestructionConfig { get; }
        GameplaySettingsConfig GameplaySettingsConfig { get; }
        EnviromentExplosionsConfig EnviromentExplosionsConfig { get; }
        PlayerCharacterConfig[] PlayerCharacterCofigs { get; }
        MainMenuSettingsConfig MainMenuSettingsConfig { get; }

        UniTask InitializeAsync();
        EnemyConfig GetEnemy(EnemyType type);
        LevelConfig GetLevel(string key);
        ForwardFlyingBulletConfig GetBullet(ForwardFlyingBulletType type);
        HomingBulletConfig GetBullet(HomingBulletType type);
        TankConfig GetTank(uint level);
        TankSkinConfig GetSkin(string id);
        DecalConfig GetDecal(string id);
        MuzzleConfig GetMuzzle(MuzzleType type);
        LevelsSequenceConfig GetLevelsSequence(BiomeType type);
        PlayerCharacterConfig GetPlayerCharacter(string id);
    }
}
