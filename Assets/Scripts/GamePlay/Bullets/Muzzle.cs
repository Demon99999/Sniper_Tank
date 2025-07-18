using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Scripts.GamePlay.Bullets
{
    public class Muzzle : MonoBehaviour
    {
        public void SetLifeTime(float lifeTime)
        {
            Destroy(gameObject, lifeTime);
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Muzzle>>
        {
        }
    }
}
