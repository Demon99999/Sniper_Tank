using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Services.StateMachine
{
    public interface IState : IExitableState
    {
        UniTask Enter();
    }
}