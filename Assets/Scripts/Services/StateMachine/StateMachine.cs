using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Scripts.Services.StateMachine
{
    public abstract class StateMachine : IStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;

        private IExitableState _currentState;

        public StateMachine()
        {
            _states = new Dictionary<Type, IExitableState>();
        }

        protected ReadOnlyArray<IExitableState> States => _states.Values.ToArray();

        public void Enter<TState>()
            where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload)
            where TState : class, IPayloadState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        public void RegisterState<TState>(TState state)
            where TState : IExitableState
        {
            _states.Add(typeof(TState), state);
        }

        private TState GetState<TState>()
            where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }

        private TState ChangeState<TState>()
            where TState : class, IExitableState
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            TState state = GetState<TState>();

            _currentState = state;

            return state;
        }
    }
}