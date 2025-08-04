using Assets.Scripts.Data;
using Assets.Scripts.GamePlay.Tanks;
using Assets.Scripts.Infrastructure.Factoris.TankFactory;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.MenuScene.CharacterPoints
{
    public class WorldSelectedTankPoint : SelectedTankPoint
    {
        private readonly Vector3 _offset = new Vector3(0, 2, 0);

        [SerializeField] private TMP_Text _tankLevelValue;
        [SerializeField] private float _fallHeight = 10f;
        [SerializeField] private float _fallDuration = 1f;

        private TankShootingWrapper _currentTankWrapper;
        private bool _isDestroyed;

        private PlayerCharacter _playerCharacter;
        private Tank _tank;

        protected override async UniTask OnStart()
        {
            await base.OnStart();
            PersistentProgressService.Progress.CharacterSkinChanged += OnCharacterSkinChanged;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PersistentProgressService.Progress.CharacterSkinChanged -= OnCharacterSkinChanged;
        }

        protected override async UniTask<GameObject> CreateTank(
            TankData tankData,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            ITankFactory tankFactory)
        {
            if (_currentTankWrapper != null)
            {
                Destroy(_currentTankWrapper.gameObject);
                _currentTankWrapper = null;
            }

            Vector3 targetPosition = position;
            Vector3 startPosition = targetPosition + Vector3.up * _fallHeight;

            _currentTankWrapper = await tankFactory.CreateTankShootingWrapper(
                tankData.Level,
                startPosition,
                rotation,
                parent);

            if (_isDestroyed || _currentTankWrapper == null)
                return null;

            _tank = await tankFactory.CreateTank(
                tankData.Level,
                startPosition,
                _currentTankWrapper.transform.rotation,
                _currentTankWrapper.transform,
                tankData.SkinId,
                tankData.DecalId,
                true);

            if (_isDestroyed || _tank == null)
                return null;

            _playerCharacter = await CreatePlayerCharacter(tankFactory, _tank);

            if (_isDestroyed || _playerCharacter == null)
                return null;

            _currentTankWrapper.SetBulletPoints(_tank.BulletPoints);
            _tankLevelValue.text = tankData.Level.ToString();

            await AnimateTankFall(_currentTankWrapper.gameObject, targetPosition);

            if (_isDestroyed)
                return null;

            if (tankData.IsFirstAppearance && tankData.Level > 1)
            {
                _currentTankWrapper.TryAutoShoot();
                tankData.IsFirstAppearance = false;
            }

            return _currentTankWrapper.gameObject;
        }

        protected override Transform GetParent()
        {
            return TankPoint.transform;
        }

        private async void OnCharacterSkinChanged(string characterId)
        {
            if (_playerCharacter != null)
                Destroy(_playerCharacter.gameObject);

            _playerCharacter = await CreatePlayerCharacter(TankFactory, _tank);
        }

        private async UniTask<PlayerCharacter> CreatePlayerCharacter(ITankFactory tankFactory, Tank tank)
        {
            return await tankFactory.CreatePlayerCharacter(
                PersistentProgressService.Progress.SelectedPlayerCharacterId,
                tank.transform.position + _offset,
                tank.transform.rotation,
                tank.transform);
        }

        private async UniTask AnimateTankFall(GameObject tankObject, Vector3 targetPosition)
        {
            if (tankObject == null)
                return;

            float elapsedTime = 0f;
            Vector3 startPosition = tankObject.transform.position;

            while (elapsedTime < _fallDuration)
            {
                if (tankObject == null || _isDestroyed)
                    return;

                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / _fallDuration);
                progress = EaseOutQuad(progress);

                tankObject.transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
                await UniTask.Yield();
            }

            if (tankObject != null)
            {
                tankObject.transform.position = targetPosition;
            }
        }

        private float EaseOutQuad(float x)
        {
            return 1 - (1 - x) * (1 - x);
        }
    }
}