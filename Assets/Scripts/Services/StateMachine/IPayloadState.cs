namespace Assets.Scripts.Services.StateMachine
{
    public interface IPayloadState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }
}