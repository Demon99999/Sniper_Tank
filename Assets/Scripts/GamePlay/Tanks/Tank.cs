using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Scripts.GamePlay.Tanks
{
    public class Tank : MonoBehaviour
    {
        [SerializeField] private TankSkin _tankSkin;
        [SerializeField] private Decals _decals;
        [SerializeField] private Transform[] _bulletPoints;
        [SerializeField] private Transform _turret;

        public uint Level { get; private set; }
        public Transform[] BulletPoints => _bulletPoints;
        public Transform Turret => _turret;

        public void Initialize(Material skinMaterial, Material decalMaterial, bool isDecalsChangable)
        {
            _tankSkin.SetMaterial(skinMaterial);
            _decals.Initialize(decalMaterial, isDecalsChangable);
        }

        public void SetLevel(uint level)
        {
            Level = level;
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, Transform, UniTask<Tank>>
        {
        }
    }
}