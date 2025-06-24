namespace Assets.Scripts.Services.StateMachine
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}