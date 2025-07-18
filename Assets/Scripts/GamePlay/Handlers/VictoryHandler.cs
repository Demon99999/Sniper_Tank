using System;
using System.Collections.Generic;
using Assets.Scripts.GamePlay.Enemis;
using Assets.Scripts.Services.CoroutineRunnerServices;
using Assets.Scripts.Services.InputService;
using Assets.Scripts.Services.StaticData;

namespace Assets.Scripts.GamePlay.Handlers
{
    public class VictoryHandler : GameplayHandler, IDisposable
    {
        private readonly IInputService _inputService;

        private List<Enemy> _enemies;
        private int _destructedEnemiesCount;

        public VictoryHandler(ICoroutineRunner coroutineRunner, IStaticDataService staticDataService, IInputService inputService)
            : base(coroutineRunner, staticDataService)
        {
            _enemies = new List<Enemy>();
            _inputService = inputService;
        }

        public event Action<int> DestructedEnemiesCountChanger;
        public event Action Woned;
        public event Action WindowsSwithed;

        public int MaxEnemiesCount { get; private set; }
        public bool IsWoned => _destructedEnemiesCount >= MaxEnemiesCount;
        public IReadOnlyList<Enemy> Enemies => _enemies;

        public void AddEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);
            enemy.Destructed += OnEnemyDestructed;

            MaxEnemiesCount++;
        }

        public void Dispose()
        {
            foreach (Enemy enemy in _enemies)
                enemy.Destructed -= OnEnemyDestructed;
        }

        private void OnEnemyDestructed()
        {
            _destructedEnemiesCount++;
            DestructedEnemiesCountChanger?.Invoke(_destructedEnemiesCount);

            if (IsWoned)
            {
                Woned?.Invoke();
                _inputService.SetActive(false);
                StartTimer(callback: () => WindowsSwithed?.Invoke());
            }
        }
    }
}