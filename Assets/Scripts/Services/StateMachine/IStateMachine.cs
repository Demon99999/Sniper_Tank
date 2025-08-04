namespace Assets.Scripts.Services.StateMachine
{
    public interface IStateMachine
    {
        void Enter<TState, TPayload>(TPayload payload)
            where TState : class, IPayloadState<TPayload>;

        void Enter<TState>()
            where TState : class, IState;
    }
}