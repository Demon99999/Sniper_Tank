using System;
using System.Linq;
using Assets.Scripts.Data;
using Assets.Scripts.Services.PersistentProgressServices;
using Assets.Scripts.Services.SaveLoadProgressServices;
using Assets.Scripts.Services.StaticData;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Services.StateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;

        public LoadProgressState(
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService,
            GameStateMachine gameStateMachine,
            IStaticDataService staticDataService)
        {
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
        }

        public UniTask Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<MainMenuState>();

            return default;
        }

        public UniTask Exit()
        {
            return default;
        }

        private void LoadProgressOrInitNew()
        {
            _persistentProgressService.Progress = _saveLoadService.LoadProgress() ?? CreateNewProgress();
        }

        private PlayerProgress CreateNewProgress()
        {
            DecalData[] decalDatas = _staticDataService.DecalConfigs.Select(config => new DecalData(config.Id, config.IsUnlockedOnStart)).ToArray();
            string startDecal = decalDatas.First(decal => decal.IsUnlocked).Id;
            TankData[] tankDatas = _staticDataService.TankConfigs.Select(config => new TankData(config.Level, config.IsUnlockOnStart, startDecal)).ToArray();
            TankSkinData[] tankSkinDatas = _staticDataService.TankSkinConfigs.Select(config => new TankSkinData(config.Id)).ToArray();
            CharacterData[] characterSkinDatas = _staticDataService.PlayerCharacterCofigs.Select(config => new CharacterData(config.Id, config.IsUnlockedOnStart)).ToArray();

            PlayerProgress progress = new PlayerProgress(tankDatas, tankSkinDatas, decalDatas, 0, characterSkinDatas, _staticDataService.MainMenuSettingsConfig.StartPrice);

            return progress;
        }
    }
}
