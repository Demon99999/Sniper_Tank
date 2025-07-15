using Zenject;

namespace Assets.Scripts.Services.StateMachine
{
    public class StatesFactory
    {
        private readonly IInstantiator _instantiator;

        public StatesFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public TState Create<TState>()
            where TState : IExitableState
        {
            return _instantiator.Instantiate<TState>();
        }
    }
}