using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Services.AssetManagementServices
{
    public interface IAssetProvider
    {
        UniTask<TAsset> Load<TAsset>(string key)
            where TAsset : class;
        UniTask<TAsset> Load<TAsset>(AssetReferenceGameObject reference)
            where TAsset : class;
        UniTask<TAsset[]> LoadAll<TAsset>(List<string> keys)
            where TAsset : class;
        UniTask InitializeAsync();
        UniTask WarmupAssetsByLable(string label);
        UniTask<List<string>> GetAssetsListByLabel<TAsset>(string label);
        UniTask<TAsset> Load<TAsset>(AssetReference reference);
        UniTask ReleaseAssetsByLabel(string label);
    }
}
