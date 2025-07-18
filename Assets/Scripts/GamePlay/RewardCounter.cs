using System.Linq;
using Assets.Scripts.GamePlay.Handlers;
using Assets.Scripts.Services.PersistentProgressServices;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Services.StaticData.ScriptableConfig;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class RewardCounter
    {
        private const float MinRewardModifier = 1;

        private readonly VictoryHandler _victoryHandler;
        private readonly GameplaySettingsConfig _gameplaySettings;
        private readonly IPersistentProgressService _persistentProgerssService;

        public RewardCounter(
            VictoryHandler victoryHandler,
            IStaticDataService staticDataService,
            IPersistentProgressService persistentProgerssService)
        {
            _victoryHandler = victoryHandler;
            _gameplaySettings = staticDataService.GameplaySettingsConfig;
            _persistentProgerssService = persistentProgerssService;
        }

        public uint GetReward()
        {
            int destructionEnemiesCount = _victoryHandler.Enemies.Count(enemy => enemy.IsDestructed);

            return (uint)(destructionEnemiesCount * _gameplaySettings.RewardPerEnemy * Mathf.Max(
                MinRewardModifier,
                (float)_persistentProgerssService.Progress.CompletedLevelsCount * _gameplaySettings.RewardLevelModifier));
        }
    }
}
