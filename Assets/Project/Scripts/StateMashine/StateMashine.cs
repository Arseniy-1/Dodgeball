using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMashine : IStateSwitcher
{
    private List<IState> _states;
    public IState _currentState;

    public StateMashine(List<IState> states)
    {
        _states = states;

        _currentState = _states[0];
        _currentState.Enter();
    }

    public void SwitchState<T>() where T : IState
    {
        IState state = _states.FirstOrDefault(state => state is T);

        if (state == null)
            throw new ArgumentNullException(nameof(T));

        _currentState.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    public void Update() => _currentState.Update();

    public void Dispose()
    {
        foreach (var state in _states)
        {
            _currentState.Exit();

            if (state is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        _states.Clear();
        _currentState = null;
    }
}