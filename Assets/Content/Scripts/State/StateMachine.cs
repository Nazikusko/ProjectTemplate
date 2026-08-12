using UnityEngine;

public class StateMachine
{
    public State CurrentState { get; private set; }

    public void Init(State startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }

    public void SwitchState(State newState)
    {
        if (CurrentState == newState)
            return;
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        if (CurrentState == null) return;

        CurrentState.Update();
    }

    public void FixedUpdate()
    {
        if (CurrentState == null) return;

        CurrentState.FixedUpdate();
    }
}
