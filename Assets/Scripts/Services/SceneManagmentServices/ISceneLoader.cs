using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Services.SceneManagmentServices
{
    public interface ISceneLoader
    {
        UniTask Load(string scene);
    }
}