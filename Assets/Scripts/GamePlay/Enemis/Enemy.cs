using System;
using Assets.Scripts.GamePlay.Player.Aim;
using Assets.Scripts.GamePlay.Player.Wrappers;
using Assets.Scripts.Services.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Scripts.GamePlay.Enemis
{
    public abstract class Enemy : MonoBehaviour
    {
        public event Action Destructed;

        public PlayerWrapper PlayerWrapper { get; private set; }
        public IShootedAiming Aiming { get; private set; }
        public LayerMask LayerMask { get; private set; }
        public bool IsDestructed { get; private set; }

        [Inject]
        private void Construct(PlayerWrapper playerWrapper, IShootedAiming aiming, IStaticDataService staticDataService)
        {
            PlayerWrapper = playerWrapper;
            Aiming = aiming;
            LayerMask = staticDataService.GameplaySettingsConfig.EnemyLayerMask;

            IsDestructed = false;
        }

        protected void OnDestructed()
        {
            IsDestructed = true;
            Destructed?.Invoke();
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Enemy>>
        {
        }
    }
}
