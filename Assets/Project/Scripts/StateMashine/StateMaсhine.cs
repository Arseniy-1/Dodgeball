using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMaсhine : IStateSwitcher
{
    public List<IState> _states;
    public IState _currentState;

    public StateMaсhine(List<IState> states)
    {
        _states = states;

        _currentState = _states[0];
        _currentState.Enter();
    }

    public void SwitchState<T>() where T : IState
    {
        IState state = _states.FirstOrDefault(state => state is T);

        if (_states.Count == 0)
            Debug.Log("Zero States count");

        if (state == null)
            throw new ArgumentNullException(nameof(T));

        _currentState.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    public void Update() => _currentState.Update();

    public void Dispose()
    {
        _currentState.Exit();

        foreach (var state in _states)
        {
            if (state is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        _states.Clear();
        _currentState = null;
    }
}