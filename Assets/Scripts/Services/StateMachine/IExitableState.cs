using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Services.StateMachine
{
    public interface IExitableState
    {
        UniTask Exit();
    }
}