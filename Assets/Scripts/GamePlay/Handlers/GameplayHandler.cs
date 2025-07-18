using System;
using System.Collections;
using Assets.Scripts.Services.CoroutineRunnerServices;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Services.StaticData.ScriptableConfig;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Handlers
{
    public class GameplayHandler
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameplaySettingsConfig _gameplaySettings;

        public GameplayHandler(ICoroutineRunner coroutineRunner, IStaticDataService staticDataService)
        {
            _coroutineRunner = coroutineRunner;
            _gameplaySettings = staticDataService.GameplaySettingsConfig;
        }

        protected void StartTimer(Action callback)
        {
            _coroutineRunner.StartCoroutine(Timer(callback));
        }

        private IEnumerator Timer(Action callback)
        {
            yield return new WaitForSeconds(_gameplaySettings.WindowsSwitchDeley);

            callback?.Invoke();
        }
    }
}
