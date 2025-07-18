using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Services.StaticData.ScriptableConfig;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Ui.Game.VictoryWindow
{
    public class MoneyEffect : MonoBehaviour
    {
        [SerializeField] private MoneyParticle _moneyParticlePrefab;
        [SerializeField] private Transform _starPoint;
        [SerializeField] private Transform _targetPoint;
        [SerializeField] private int _particlesCount;

        private AnimationsConfig _animationConfig;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _animationConfig = staticDataService.AnimationsConfig;
        }

        public void Play()
        {
            for (int i = 0; i < _particlesCount; i++)
            {
                MoneyParticle moneyParticle = Instantiate(_moneyParticlePrefab, _starPoint.position, Quaternion.identity, transform);
                moneyParticle.Initialize(_starPoint.position, _targetPoint.position, _animationConfig);
            }
        }
    }
}
