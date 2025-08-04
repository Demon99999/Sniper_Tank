using System.Collections;
using Assets.Scripts.GamePlay.Handlers;
using Assets.Scripts.GamePlay.Player.Aim;
using Assets.Scripts.GamePlay.Player.Wrappers;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Services.StaticData.ScriptableConfig;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GamePlay.Enemis.EnemyShooting
{
    public abstract class EnemyShooting : MonoBehaviour
    {
        private readonly Vector3 _targetOffset = new Vector3(0, 2, 0);

        [SerializeField] private Enemy _enemy;
        [SerializeField] private AudioSource _audioSource;

        private GameplaySettingsConfig _gameplaySettings;
        private IShootedAiming _aiming;
        private DefeatHandler _defeatHandler;

        private bool _isShootingStarted;
        private bool _isPlayerDefeated;

        public PlayerWrapper PlayerWrapper { get; private set; }

        protected bool IsShooted { get; private set; }
        protected virtual bool CanShoot => _isPlayerDefeated == false;
        protected AudioSource AudioSource => _audioSource;

        [Inject]
        private void Construct(
            IStaticDataService staticDataService,
            PlayerWrapper playerWrapper,
            IShootedAiming aiming,
            DefeatHandler defeatHandler)
        {
            _gameplaySettings = staticDataService.GameplaySettingsConfig;
            PlayerWrapper = playerWrapper;
            _aiming = aiming;
            _defeatHandler = defeatHandler;

            _isShootingStarted = false;
            _isPlayerDefeated = false;
            IsShooted = false;

            _aiming.Shooted += OnPlayerTankAttacked;
            _defeatHandler.Defeated += OnPlayerDefeated;
            _defeatHandler.ProgressRecovered += OnProgressRecovery;
            _enemy.Destructed += OnEnemyDestructed;
        }

        private void OnDestroy()
        {
            _aiming.Shooted -= OnPlayerTankAttacked;
            _defeatHandler.Defeated -= OnPlayerDefeated;
            _defeatHandler.ProgressRecovered -= OnProgressRecovery;
            _enemy.Destructed -= OnEnemyDestructed;
        }

        protected Quaternion GetShootingRotation()
        {
            Vector2 randomOffset = Random.insideUnitCircle * _gameplaySettings.EnemyScatter;

            Vector3 targetPosition = PlayerWrapper.transform.position + _targetOffset +
                new Vector3(randomOffset.x, randomOffset.y, 0);

            return Quaternion.LookRotation((targetPosition - GetCurrentShootingPosition()).normalized);
        }

        protected abstract Vector3 GetCurrentShootingPosition();

        protected virtual void StartShooting() =>
            StartCoroutine(Shooter());

        protected virtual void OnEnemyDestructed() =>
            IsShooted = false;

        protected void OnShooted()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();

                if (_audioSource == null)
                {
                    return;
                }
            }

            _audioSource.Play();
        }

        private void OnPlayerTankAttacked()
        {
            if (_isShootingStarted)
                return;

            _isShootingStarted = true;
            IsShooted = true;

            StartShooting();
        }

        private void OnPlayerDefeated() =>
            _isPlayerDefeated = true;

        private void OnProgressRecovery() =>
            _isPlayerDefeated = false;

        protected abstract IEnumerator Shooter();
    }
}