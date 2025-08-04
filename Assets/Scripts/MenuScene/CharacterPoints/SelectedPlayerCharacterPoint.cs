using Assets.Scripts.GamePlay.Tanks;
using Assets.Scripts.Infrastructure.Factoris.TankFactory;
using Assets.Scripts.Services.PersistentProgressServices;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.MenuScene.CharacterPoints
{
    public class SelectedPlayerCharacterPoint : MonoBehaviour
    {
        private const string Layer = "UI";

        [SerializeField] private Transform _characterPoint;
        [SerializeField] private TankScalingAnimator _scalingAnimator;
        [SerializeField] private float _scale;

        private IPersistentProgressService _persistentPorgressService;
        private ITankFactory _tankFactory;

        private PlayerCharacter _playerCharacter;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, ITankFactory tankFactory)
        {
            _persistentPorgressService = persistentProgressService;
            _tankFactory = tankFactory;

            _persistentPorgressService.Progress.CharacterSkinChanged += OnCharacterSkinChanged;
        }

        private async void Start()
        {
            await CreatePlayerCharacter(_persistentPorgressService.Progress.SelectedPlayerCharacterId, false);
        }

        private void OnDestroy()
        {
            _persistentPorgressService.Progress.CharacterSkinChanged -= OnCharacterSkinChanged;
        }

        private async void OnCharacterSkinChanged(string id)
        {
            await CreatePlayerCharacter(id, true);
        }

        private async UniTask CreatePlayerCharacter(string id, bool needToAnimate)
        {
            if (_playerCharacter != null)
                Destroy(_playerCharacter.gameObject);

            _playerCharacter = await _tankFactory.CreatePlayerCharacter(id, _characterPoint.position, _characterPoint.rotation, _characterPoint);

            foreach (Transform transform in _playerCharacter.GetComponentsInChildren<Transform>())
                transform.gameObject.layer = LayerMask.NameToLayer(Layer);

            _playerCharacter.transform.localScale = Vector3.one * _scale;

            if (needToAnimate)
                _scalingAnimator.Play();
        }
    }
}