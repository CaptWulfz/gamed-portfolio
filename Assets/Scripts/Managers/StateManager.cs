using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateManager<T>
{
    private List<T> statesList;
    private Dictionary<T, List<T>> transitionsList;
    private Dictionary<T, Action> onStateExitDefaultActions;
    private Dictionary<T, Action> onStateEnterDefaultActions;

    private T currentState;
    public T CurrentState
    {
        get { return currentState; }
    }

    private bool lockTransitions;
    public bool LockTransitions
    {
        get { return lockTransitions; }
    }

    #region Initialization
    public void Initialize(T[] statesList, T initialState, bool lockTransitions = false)
    {
        SetupStateManager(statesList.ToList(), initialState, lockTransitions);
    }

    public void Initialize(List<T> statesList, T initialState, bool lockTransitions = false)
    {
        SetupStateManager(statesList, initialState, lockTransitions);
    }

    public void SetupStateManager(List<T> statesList, T initialState, bool lockTransitions = false)
    {
        this.statesList = statesList;
        this.currentState = initialState;
        this.lockTransitions = lockTransitions;
        if (lockTransitions)
            this.transitionsList = new Dictionary<T, List<T>>();
    }
    #endregion

    public void SwitchState(T nextState, Action onPrevStateExit = null, Action onNextStateEnter = null)
    {
        bool allowTransition = this.lockTransitions ? CheckValidTransition(nextState) : true;

        if (allowTransition)
        {
            if (this.onStateExitDefaultActions != null && this.onStateExitDefaultActions.ContainsKey(this.currentState))
                this.onStateExitDefaultActions[this.currentState]?.Invoke();
            onPrevStateExit?.Invoke();
            this.currentState = nextState;
            if (this.onStateEnterDefaultActions != null && this.onStateEnterDefaultActions.ContainsKey(this.currentState))
                this.onStateEnterDefaultActions[this.currentState]?.Invoke();
            onNextStateEnter?.Invoke();
        }
    }

    public void MakeTransition(T currentState, T nextState)
    {
        if (transitionsList == null)
            return;

        if (this.transitionsList.ContainsKey(currentState))
        {
            List<T> list = this.transitionsList[currentState];
            if (!list.Contains(nextState))
                list.Add(nextState);
        } else
        {
            List<T> list = new List<T>();
            list.Add(nextState);
            this.transitionsList.Add(currentState, list);
        }
    }

    public void AddDefaultStateEnter(T state, Action stateEnter)
    {
        if (this.onStateEnterDefaultActions == null)
            this.onStateEnterDefaultActions = new Dictionary<T, Action>();

        
        if (!this.onStateEnterDefaultActions.ContainsKey(state))
        {
            this.onStateEnterDefaultActions.Add(state, stateEnter);
        } else
        {
            this.onStateEnterDefaultActions[state] += stateEnter;
        }
    }

    public void RemoveDetaultStateEnter(T state, Action stateEnter)
    {
        if (this.onStateEnterDefaultActions.ContainsKey(state))
        {
            this.onStateEnterDefaultActions[state] -= stateEnter;
        }
    }

    public void AddDefaultStateExit(T state, Action stateExit)
    {
        if (this.onStateExitDefaultActions == null)
            this.onStateExitDefaultActions = new Dictionary<T, Action>();


        if (!this.onStateExitDefaultActions.ContainsKey(state))
        {
            this.onStateExitDefaultActions.Add(state, stateExit);
        } else
        {
            this.onStateExitDefaultActions[state] += stateExit;
        }
    }

    public void RemoveDetaultStateExit(T state, Action stateExit)
    {
        if (this.onStateExitDefaultActions.ContainsKey(state))
        {
            this.onStateExitDefaultActions[state] -= stateExit;
        }
    }

    private bool CheckValidTransition(T nextState)
    {
        if (this.transitionsList.ContainsKey(this.currentState))
        {
            List<T> transitions = this.transitionsList[this.currentState];
            return transitions.Contains(nextState);
        } else
        {
            return false;
        }
    }
}
