using Assets.Scripts.Services.StateMachine;
using Assets.Scripts.Services.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine;
        private StatesFactory _statesFactory;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, StatesFactory statesFactory)
        {
            _gameStateMachine = gameStateMachine;
            _statesFactory = statesFactory;
        }

        private void Start()
        {
            _gameStateMachine.RegisterState(_statesFactory.Create<BootstapState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<GameplayLoopState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<MainMenuState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<LoadProgressState>());

            _gameStateMachine.Enter<BootstapState>();

            DontDestroyOnLoad(this);
        }
    }
}
