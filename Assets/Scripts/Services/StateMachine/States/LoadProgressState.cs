using System;
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
            throw new NotImplementedException();
        }
    }
}
