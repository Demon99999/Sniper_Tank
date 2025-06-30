using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Services.SceneManagmentServices
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask Load(string scene)
        {
            AsyncOperationHandle<SceneInstance> handler = Addressables.LoadSceneAsync(scene, LoadSceneMode.Single, false);

            await handler.ToUniTask();
            await handler.Result.ActivateAsync().ToUniTask();
        }
    }
}
