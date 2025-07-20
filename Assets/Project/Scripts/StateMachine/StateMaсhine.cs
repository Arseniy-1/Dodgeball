using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Scripts.StateMachine
{
    public class StateMaсhine : IStateSwitcher
    {
        private Dictionary<Type, IState> _states = new();
        private IState _currentState;

        public StateMaсhine(List<IState> states)
        {
            foreach (var state in states)
                _states[state.GetType()] = state;         

            _currentState = states.First();
            _currentState.Enter();
        }

        public void SwitchState<T>() where T : IState
        {
            if (_states.TryGetValue(typeof(T), out var newState) == false)
                throw new ArgumentException($"State of type {typeof(T)} not found");

            if (newState == null)
                throw new ArgumentNullException(nameof(T));

            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Update() => _currentState.Update();

        public void Dispose()
        {
            _currentState.Exit();

            foreach (var pair in _states)
            {
                if (pair.Value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _states.Clear();
            _currentState = null;
        }
    }
}