using System;
using Assets.Scripts.Infrastructure.Factoris.MenuMain;
using Assets.Scripts.Infrastructure.Factoris.UI;
using Assets.Scripts.Services.StateMachine;
using Assets.Scripts.Services.StateMachine.States;
using Assets.Scripts.Ui.MainMenu;
using Zenject;

namespace Assets.Scripts.MenuScene
{
    public class MainMenuBootstrapper : IInitializable, IDisposable
    {
        private readonly IUiFactory _uiFactory;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IMainMenuFactory _mainMenuFactory;

        private MainMenuWindow _mainMenuWindow;

        public MainMenuBootstrapper(IUiFactory uiFactory, GameStateMachine gameStateMachine, IMainMenuFactory mainMenuFactory)
        {
            _uiFactory = uiFactory;
            _gameStateMachine = gameStateMachine;
            _mainMenuFactory = mainMenuFactory;
        }

        public void Dispose()
        {
            _mainMenuWindow.FightButtonClicked -= OnFightButtonClicked;
        }

        public async void Initialize()
        {
            await _mainMenuFactory.CreateDesk();
            await _uiFactory.CreateOptionsWindow();
            _mainMenuWindow = await _uiFactory.CreateMainMenu();

            _mainMenuWindow.FightButtonClicked += OnFightButtonClicked;
        }

        private void OnFightButtonClicked()
        {
            _gameStateMachine.Enter<GameplayLoopState>();
        }
    }
}
